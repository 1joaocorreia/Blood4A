package main

import (
	"bufio"
	"database/sql"
	"fmt"
	"math/rand"
	"os"
	"strconv"
	"strings"

	_ "github.com/mattn/go-sqlite3"
)

const data_files_size_value int = 200

var names [data_files_size_value]string
var surnames [data_files_size_value]string

type Localizacao struct {
	cep        string
	logradouro string
	bairro     string
	cidade     string
	estado     string
	uf         int
}

type Agente struct {
	id_agente    int
	nome_agente  string
	cnpj_agente  string
	email_agente string
	senha_agente string
}

type Clinica struct {
	id_clinica   int
	nome_clinica string
	cnpj_clinica string
	cep_location string // Localizacao cep
}

type AberturaFechamento struct {
	referente_a        int // Clinica id_clinic
	horario_abertura   string
	horario_fechamento string
	dia_da_semana      string
}

type Escolaridade struct {
	id_escolaridade        int
	grau_escolaridade      string
	descricao_escolaridade string
}

type Doador struct {
	id_doador              int
	nome_doador            string
	data_nascimento_doador string
	cpf_doador             string
	telefone_doador        string
	tipo_sanguineo_doador  string
	cep_location           string // Localizacao cep
	id_escolaridade        int    // Escolaridade id_escolaridade
}

type Doacao struct {
	id_doacao   int
	id_agente   int // Agente id_agente
	id_doador   int // Doador id_doador
	id_clinica  int // Clinica id_clinica
	data_doacao string
	hora_doacao string
}

var dias_da_semana []string = []string{
	"Segunda Feira",
	"Terca Feira",
	"Quarta Feira",
	"Quinta Feira",
	"Sexta Feira",
}

func init() {

	var err error

	err = get_lines_from_file("data/nomes.txt", &names)
	if err != nil {
		fmt.Println("Not possible to get data from nomes.txt")
		fmt.Println(err.Error())
		os.Exit(1)
	}

	err = get_lines_from_file("data/apelidos.txt", &surnames)
	if err != nil {
		fmt.Println("Not possible to get data from apelidos.txt")
		fmt.Println(err.Error())
		os.Exit(1)
	}

}

func get_lines_from_file(filepath string, output *[data_files_size_value]string) error {

	file, err := os.Open(filepath)
	if err != nil {
		fmt.Println("Not possible to open the file " + filepath)
		return err
	}

	defer file.Close()

	var scanner bufio.Scanner = *bufio.NewScanner(file)
	var counter int = 0
	for scanner.Scan() {
		line := scanner.Text()

		if line == "" {
			continue
		}

		if counter < data_files_size_value {

			output[counter] = line
			counter++
			continue

		}

	}

	return nil
}

func generate_random_name() string {

	var picked_value = randRange(0, data_files_size_value-1)

	return names[picked_value]
}

func generate_random_surname() string {

	var picked_value = randRange(0, data_files_size_value-1)

	return surnames[picked_value]
}

func replace_x_for_int(model string, min_int int, max_int int) string {

	var model_builder strings.Builder

	for _, letter := range model {

		if letter == 'x' {
			model_builder.WriteString(strconv.Itoa(randRange(min_int, max_int)))
			continue
		} else {
			model_builder.WriteRune(letter)
			continue
		}

	}

	return model_builder.String()

}

func exists_on_db(db *sql.DB, table string, condition string) (bool, error) {
	queryStr := fmt.Sprintf("SELECT COUNT(1) FROM %s WHERE %s", table, condition)

	var count int
	err := db.QueryRow(queryStr).Scan(&count)

	if err != nil && err != sql.ErrNoRows {
		return false, fmt.Errorf("erro ao executar query de existência: %w", err)
	}

	return count > 0, nil
}

func generate_random_cnpj() string {
	return replace_x_for_int("xx.xxx.xxx/xxxx-xx", 0, 9)
}

func generate_random_cpf() string {

	return replace_x_for_int("xxx.xxx.xxx-xx", 0, 9)
}

func generate_random_location(db *sql.DB, estado string) (Localizacao, error) {

	var generated_location Localizacao = Localizacao{}

	var cep string

	for {
		cep = replace_x_for_int("xxxxx-xxx", 0, 9)
		exists, err := exists_on_db(db, "Localizacoes", fmt.Sprintf("cep = '%s'", cep))
		if err != nil {
			return Localizacao{}, err
		}
		if exists {
			continue
		}
		break
	}

	generated_location.cep = cep
	generated_location.logradouro = "Unknown"
	generated_location.bairro = fmt.Sprintf("Bairro Teste %d%d%d", randRange(0, 100), randRange(0, 200), randRange(0, 300))
	generated_location.cidade = fmt.Sprintf("Cidade Teste %d%d%d", randRange(0, 100), randRange(0, 200), randRange(0, 300))
	generated_location.estado = estado
	generated_location.uf = randRange(0, 1000)

	return generated_location, nil

}

func generate_random_agente(agente_id int) Agente {

	var generated_agente Agente = Agente{}

	generated_agente.id_agente = agente_id
	generated_agente.nome_agente = generate_random_name()
	generated_agente.cnpj_agente = generate_random_cnpj()
	generated_agente.email_agente = fmt.Sprintf("Random Email %s%s@hotmail.com", strings.ToLower(generate_random_name()), strings.ToLower(generate_random_surname()))
	generated_agente.senha_agente = fmt.Sprintf("Random Password: %d%d%d", randRange(0, 100), randRange(0, 100), randRange(0, 100))

	return generated_agente
}

func generate_random_clinica(clinica_id int, cep string) Clinica {

	var generated_clinica Clinica = Clinica{}

	generated_clinica.id_clinica = clinica_id
	generated_clinica.nome_clinica = fmt.Sprintf("Clínica %s %s", generate_random_surname(), generate_random_surname())
	generated_clinica.cnpj_clinica = generate_random_cnpj()
	generated_clinica.cep_location = cep

	return generated_clinica

}

func generate_random_escolaridade(escolaridade_id int) Escolaridade {

	var generated_escolaridade Escolaridade = Escolaridade{}

	var tipos_escolaridade = []string{
		"Infantil",
		"Fundamental",
		"Superior",
		"Pós Graduação",
		"Mestrado",
		"Doutorado",
	}

	generated_escolaridade.id_escolaridade = escolaridade_id
	generated_escolaridade.grau_escolaridade = tipos_escolaridade[randRange(0, len(tipos_escolaridade)-1)]
	generated_escolaridade.descricao_escolaridade = "No Description Available..."

	return generated_escolaridade

}

func generate_random_doador(doador_id int, cep string, escolaridade int) Doador {

	var generated_doador Doador = Doador{}

	var tipos_de_sangue = []string{
		"A",
		"B",
		"AB",
		"O",
	}

	generated_doador.id_doador = doador_id
	generated_doador.nome_doador = generate_random_name()
	generated_doador.data_nascimento_doador = fmt.Sprintf("%d/%d/%d", randRange(1, 32), randRange(1, 13), randRange(2000, 2100))
	generated_doador.cpf_doador = generate_random_cpf()
	generated_doador.telefone_doador = replace_x_for_int("(xx) xxxxx-xxxx", 0, 9)
	generated_doador.tipo_sanguineo_doador = tipos_de_sangue[randRange(0, len(tipos_de_sangue))]
	generated_doador.cep_location = cep
	generated_doador.id_escolaridade = escolaridade

	return generated_doador

}

func generate_random_doacao(doacao_id int, agente int, doador int, clinica int) Doacao {

	var generated_doacao Doacao = Doacao{}

	generated_doacao.id_doacao = doacao_id
	generated_doacao.id_agente = agente
	generated_doacao.id_doador = doador
	generated_doacao.id_clinica = clinica
	generated_doacao.data_doacao = fmt.Sprintf("%d/%d/%d", randRange(1, 32), randRange(1, 13), randRange(2000, 2100))
	generated_doacao.hora_doacao = fmt.Sprintf("%d:%d", randRange(10, 24), randRange(1, 60))

	return generated_doacao
}

func randRange(min, max int) int {
	return rand.Intn(max-min) + min
}

func populate_db(db_path string) bool {

	// CONNECTING TO DB --------------------------------------------------------------
	db, err := sql.Open("sqlite3", db_path)
	if err != nil {
		fmt.Printf("Not possible to connect to the specified path: < %s >\n", db_path)
		fmt.Println(err.Error())
		return false
	}

	defer db.Close()
	// END OF CONNECTION TO DB --------------------------------------------------------------

	// CHANGE THOSE CONSTANTS TO CHANGE THE GENERATED DATA!
	// ------------------------------------------------------
	const quantia_de_clinicas = 200

	const media_agentes_por_clinica = 4
	const quantia_de_agentes = quantia_de_clinicas / media_agentes_por_clinica

	const media_de_doacoes_por_clinica = 50
	const quantia_de_doacoes = quantia_de_clinicas * media_de_doacoes_por_clinica

	const media_de_doacoes_por_doador = 2
	const quantia_de_doadores = quantia_de_doacoes / media_de_doacoes_por_doador

	const quantia_de_escolaridades = 10

	const quantia_de_clinicas_por_estado = 10

	const quantia_de_localizacoes = quantia_de_clinicas

	const dias_por_semana = 5
	const quantia_de_horarios = quantia_de_clinicas * dias_por_semana
	// ------------------------------------------------------

	var localizacoes = [quantia_de_localizacoes]Localizacao{}
	var escolaridades = [quantia_de_escolaridades]Escolaridade{}
	var agentes = [quantia_de_agentes]Agente{}
	var clinicas = [quantia_de_clinicas]Clinica{}
	var horarios = [quantia_de_horarios]AberturaFechamento{}
	var doadores = [quantia_de_doadores]Doador{}
	var doacoes = [quantia_de_doacoes]Doacao{}

	// GERANDO DADOS DE LOCALIZAÇÃO
	for i := 0; i < quantia_de_clinicas; i += quantia_de_clinicas_por_estado {

		// 1 cada 10 registros
		var estado = fmt.Sprintf("Estado %d%d%d", randRange(0, 20), randRange(0, 20), randRange(0, 20))

		for i2 := i; i2 < i+quantia_de_clinicas_por_estado; i2++ {
			localizacoes[i2], err = generate_random_location(db, estado)
			if err != nil {
				fmt.Println(err.Error())
				return false
			}
		}

	}

	for i := 0; i < quantia_de_clinicas; i += quantia_de_clinicas_por_estado {

		for i2 := i; i2 < i+quantia_de_clinicas_por_estado; i2++ {

			clinicas[i2] = generate_random_clinica(i2+1, localizacoes[i2].cep)

		}

	}

	var iclinica = 0
	var idiasemana = 0
	// 0... 5... 10
	for i := 0; i < quantia_de_horarios; i += dias_por_semana {
		// 0, 1, 2, 3, 4, 5... dias da semana (segunda - sexta)
		for ihorario := i; ihorario < i+dias_por_semana; ihorario++ {
			horarios[ihorario] = AberturaFechamento{}
			horarios[ihorario].referente_a = clinicas[iclinica].id_clinica
			horarios[ihorario].horario_abertura = fmt.Sprintf("%d:%d", randRange(10, 24), randRange(0, 60))
			horarios[ihorario].horario_fechamento = fmt.Sprintf("%d:%d", randRange(10, 24), randRange(0, 60))
			horarios[ihorario].dia_da_semana = dias_da_semana[idiasemana]
			idiasemana++
		}
		idiasemana = 0 // zerando esse contador pra recomeçar por segunda-feira no proximo loop
		iclinica++     // +1 apenas após a geração de 5 dias úteis para uma única clínica

	}

	for i := 0; i < quantia_de_escolaridades; i++ {
		escolaridades[i] = generate_random_escolaridade(i + 1)
	}

	for i := 0; i < quantia_de_agentes; i++ {
		agentes[i] = generate_random_agente(i + 1)
	}

	for i := 0; i < quantia_de_doadores; i++ {
		doadores[i] = generate_random_doador(i+1, localizacoes[randRange(0, quantia_de_localizacoes)].cep, escolaridades[randRange(0, quantia_de_escolaridades)].id_escolaridade)
	}

	var offset = 0
	for i := 0; i < quantia_de_clinicas; i++ {
		// Adicionar 50 valores as doacoes com o clinicas[i]
		for i2 := 0; i2 < media_de_doacoes_por_clinica; i2++ {
			doacoes[offset] = generate_random_doacao(offset+1, agentes[randRange(0, quantia_de_agentes)].id_agente, doadores[randRange(0, quantia_de_doadores)].id_doador, clinicas[i].id_clinica)
			offset++
		}
	}

	fmt.Println("Generation of random data was completed")
	fmt.Println("Populating the Database...")

	// INSERTING INTO DB -------------------------------------------------------
	var error_count = 0

	stmt, err := db.Prepare("INSERT INTO Localizacoes VALUES (?, ?, ?, ?, ?, ?)")
	if err != nil {
		fmt.Println(err.Error())
		return false
	}
	for _, localizacao := range localizacoes {

		_, err = stmt.Exec(
			localizacao.cep,
			localizacao.logradouro,
			localizacao.bairro,
			localizacao.cidade,
			localizacao.estado,
			localizacao.uf,
		)
		if err != nil {
			error_count += 1
		}

	}
	fmt.Println("Localizacoes TABLE FINISHED INITIALIZATION PROCESS ✅")
	fmt.Printf("Total of [ %d ] errors ocurred\n\n", error_count)
	error_count = 0

	stmt, err = db.Prepare("INSERT INTO Escolaridade VALUES (?, ?, ?)")
	if err != nil {
		fmt.Println(err.Error())
		return false
	}
	for _, escolaridade := range escolaridades {

		_, err = stmt.Exec(
			escolaridade.id_escolaridade,
			escolaridade.grau_escolaridade,
			escolaridade.descricao_escolaridade,
		)
		if err != nil {
			error_count += 1
		}

	}
	fmt.Println("Escolaridade TABLE FINISHED INITIALIZATION PROCESS ✅")
	fmt.Printf("Total of [ %d ] errors ocurred\n\n", error_count)
	error_count = 0

	stmt, err = db.Prepare("INSERT INTO Agentes VALUES (?, ?, ?, ?, ?)")
	if err != nil {
		fmt.Println(err.Error())
		return false
	}
	for _, agente := range agentes {

		_, err = stmt.Exec(
			agente.id_agente,
			agente.nome_agente,
			agente.cnpj_agente,
			agente.email_agente,
			agente.senha_agente,
		)
		if err != nil {
			error_count += 1
		}

	}
	fmt.Println("Agentes TABLE FINISHED INITIALIZATION PROCESS ✅")
	fmt.Printf("Total of [ %d ] errors ocurred\n\n", error_count)
	error_count = 0

	stmt, err = db.Prepare("INSERT INTO Clinicas VALUES (?, ?, ?, ?)")
	if err != nil {
		fmt.Println(err.Error())
		return false
	}
	for _, clinica := range clinicas {

		_, err = stmt.Exec(
			clinica.id_clinica,
			clinica.nome_clinica,
			clinica.cnpj_clinica,
			clinica.cep_location,
		)
		if err != nil {
			error_count += 1
		}

	}
	fmt.Println("Clinicas TABLE FINISHED INITIALIZATION PROCESS ✅")
	fmt.Printf("Total of [ %d ] errors ocurred\n\n", error_count)
	error_count = 0

	stmt, err = db.Prepare("INSERT INTO AberturaFechamento VALUES (?, ?, ?, ?)")
	if err != nil {
		fmt.Println(err.Error())
		return false
	}
	for _, horario := range horarios {

		_, err = stmt.Exec(
			horario.referente_a,
			horario.horario_abertura,
			horario.horario_fechamento,
			horario.dia_da_semana,
		)
		if err != nil {
			error_count += 1
		}

	}
	fmt.Println("AberturaFechamento TABLE FINISHED INITIALIZATION PROCESS ✅")
	fmt.Printf("Total of [ %d ] errors ocurred\n\n", error_count)
	error_count = 0

	stmt, err = db.Prepare("INSERT INTO Doadores VALUES (?, ?, ?, ?, ?, ?, ?, ?)")
	if err != nil {
		fmt.Println(err.Error())
		return false
	}
	for _, doador := range doadores {

		_, err = stmt.Exec(
			doador.id_doador,
			doador.nome_doador,
			doador.data_nascimento_doador,
			doador.cpf_doador,
			doador.telefone_doador,
			doador.tipo_sanguineo_doador,
			doador.cep_location,
			doador.id_escolaridade,
		)
		if err != nil {
			error_count += 1
		}

	}
	fmt.Println("Doadores TABLE FINISHED INITIALIZATION PROCESS ✅")
	fmt.Printf("Total of [ %d ] errors ocurred\n\n", error_count)
	error_count = 0

	fmt.Printf("Populating the Doacoes TABLE, this might take some time since < %d > rows will be created\n", quantia_de_doacoes)
	stmt, err = db.Prepare("INSERT INTO Doacoes VALUES (?, ?, ?, ?, ?, ?)")
	if err != nil {
		fmt.Println(err.Error())
		return false
	}
	for _, doacao := range doacoes {

		_, err = stmt.Exec(
			doacao.id_doacao,
			doacao.id_agente,
			doacao.id_doador,
			doacao.id_clinica,
			doacao.data_doacao,
			doacao.hora_doacao,
		)
		if err != nil {
			error_count += 1
		}

	}
	fmt.Println("Doacoes TABLE FINISHED INITIALIZATION PROCESS ✅")
	fmt.Printf("Total of [ %d ] errors ocurred\n\n", error_count)
	error_count = 0

	fmt.Println("[✅] PROCESS FINISHED SUCCESFULLY ☺️☺️")
	// END OF INSERTING INTO DB -------------------------------------------------------

	return true

}
