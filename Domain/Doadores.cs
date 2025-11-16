using System.ComponentModel.DataAnnotations.Schema;


namespace Blood4A.Domain;

public class Doadores
{
    [System.ComponentModel.DataAnnotations.Key]
    public int id_doador { get; set; }

    public required string nome_doador { get; set; }

    public required string data_nascimento_doador { get; set; }

    public required string cpf_doador { get; set; }

    public required string telefone_doador { get; set; }

    public required string tipo_sanguineo_doador { get; set; }

    // FK -----------------------------------------------
    public required string cep_location { get; set; }

    public required int id_escolaridade { get; set; }

    [ForeignKey("cep_location")]
    public Localizacoes? obj_cep_location { get; set; }

    [ForeignKey("id_escolaridade")]
    public Escolaridade? obj_id_escolaridade { get; set; }
    // FK -----------------------------------------------


}