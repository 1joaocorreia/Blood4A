using System.ComponentModel.DataAnnotations.Schema;

namespace Blood4A.Models;

public class Clinicas
{
    [System.ComponentModel.DataAnnotations.Key]
    public int Id_Clinica { get; set; }

    public string Nome_Clinica { get; set; }

    public string Cnpj_Clinica { get; set; }
    
    // FK -----------------------------------------------
    public string cep_location { get; set; }
    
    [ForeignKey("cep_location")]
    public Localizacoes obj_cep_location { get; set; }
    // FK -----------------------------------------------

}