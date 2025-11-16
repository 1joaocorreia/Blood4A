namespace Blood4A.Domain;

public class Agentes
{
    [System.ComponentModel.DataAnnotations.Key]
    public int id_agente { get; set; }

    public required string nome_agente { get; set; }

    public required string cnpj_agente { get; set; }

    public required string email_agente { get; set; }
    
    public required string senha_agente { get; set; }

}