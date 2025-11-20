const host = "localhost"
const port = 7298
const rootaddress = `https://${host}:${port}`

class _cLocation {
    constructor(cep, logradouro, bairro, cidade, estado, uf) {
        this.cep = cep;
        this.logradouro = logradouro;
        this.bairro = bairro;
        this.cidade = cidade;
        this.estado = estado;
        this.uf = uf;
    }
}

class Clinica {
    constructor(id_clinica, nome_clinica, cnpj_clinica, cep_location) {
        this.id_clinica = id_clinica;
        this.nome_clinica = nome_clinica;
        this.cnpj_clinica = cnpj_clinica;
        this.cep_location = cep_location;
    }
}

class Horario {
    constructor(referente_a, abertura, fechamento, dia_da_semana) {
        this.referente_a = referente_a;
        this.abertura = abertura;
        this.fechamento = fechamento;
        this.dia_da_semana = dia_da_semana;
    }
}

function create_new_estado_category(estado) {
    return `
    <div class="estado" id="${estado.toLowerCase()}">
        <h1><a href="${rootaddress}/home/state/${estado}">Estado < ${estado} ></a></h1>
    </div>
    `
}

function create_new_clinic_card(clinica_obj) {
    return `
        <div class="clinic_card" id="clinicid_${clinica_obj.id_clinica}">
            <div class="left">
                Clinica <b>${clinica_obj.nome_clinica}</b>
                Cep: <b>${clinica_obj.cep_location.cep}</b>
                Localizaco em: ${clinica_obj.cep_location.bairro}, ${clinica_obj.cep_location.cidade}, ${clinica_obj.cep_location.estado}.
            </div>
            <div class="right">
                <a class="btn btn-secondary" href="/home/clinic/${clinica_obj.id_clinica}">MAIS INFORMAÇÕES</a>
            </div>
        </div>
    `
}

function create_error_card(message) {
    return `
        <div class="error_card">
            <h1>An error occurred</h1>
            <p>${message}</p>
        </div>
    `
}

async function carregar_clinicas() {

    try {
        const endpoint = `${rootaddress}/api/info/get_all_clinics`;
        const response = await fetch(endpoint);

        if (!response.ok) {
            $("main#clinics_container").append(
                create_error_card("Not possible to request the server")
            );
            return;
        }

        const json = await response.json();
        const json_data = json.clinicasEstado;

        if (!json_data || json_data.length == 0) {

            $("main#clinics_container").append(
                create_error_card("No more cards available")
            );
            return;

        } else {
            
            json_data.forEach( clinicas_estado => {

                $("main#clinics_container").append(
                    create_new_estado_category(clinicas_estado.estado)
                );
                
                clinicas_estado.listaDeClinicas.forEach(clinica => {

                    $(`div[id='${clinicas_estado.estado.toLowerCase()}']`).append(

                        create_new_clinic_card(new Clinica(
                            clinica.id_clinica,
                            clinica.nome_clinica,
                            clinica.cnpj_clinica,
                            new _cLocation(
                                clinica.obj_cep_location.cep,
                                clinica.obj_cep_location.logradouro,
                                clinica.obj_cep_location.bairro,
                                clinica.obj_cep_location.cidade,
                                clinica.obj_cep_location.estado,
                                clinica.obj_cep_location.uf
                            )
                        ))

                    );

                });



            } );

        }
    } catch (err) {
        console.error(err);
    }

}

carregar_clinicas();