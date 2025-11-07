using System.ComponentModel.DataAnnotations.Schema;

namespace Blood4A.Models;

public class Doadores
{
    [System.ComponentModel.DataAnnotations.Key]
    public int Id_Doador { get; set; }

    public string Nome_Doador { get; set; }

    public string Data_Nascimento_Doador { get; set; }

    public string Cpf_Doador { get; set; }

    public string Telefone_Doador { get; set; }

    public string Tipo_Sanguineo_Doador { get; set; }

    // FK -----------------------------------------------
    public string cep_location { get; set; }

    public int id_escolaridade { get; set; }

    [ForeignKey("cep_location")]
    public Localizacoes obj_cep_location { get; set; }

    [ForeignKey("id_escolaridade")]
    public Escolaridade obj_id_escolaridade { get; set; }
    // FK -----------------------------------------------


}