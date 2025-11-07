namespace Blood4A.Models;

public class Agentes
{
    [System.ComponentModel.DataAnnotations.Key]
    public int Id_Agente { get; set; }

    public string Nome_Agente { get; set; }

    public string Cnpj_Agente { get; set; }

    public string Email_Agente { get; set; }
    
    public string Senha_Agente { get; set; }

}