using System.ComponentModel.DataAnnotations.Schema;


namespace Blood4A.Domain;

public class Clinicas
{
    [System.ComponentModel.DataAnnotations.Key]
    public int id_clinica { get; set; }

    public required string nome_clinica { get; set; }

    public required string cnpj_clinica { get; set; }
    
    // FK -----------------------------------------------
    public required string cep_location { get; set; }
    
    [ForeignKey("cep_location")]
    public Localizacoes? obj_cep_location { get; set; }
    // FK -----------------------------------------------

}