namespace Blood4A.Models;

public class Escolaridade
{
    [System.ComponentModel.DataAnnotations.Key]
    public int Id_Escolaridade { get; set; }

    public string Grau_Escolaridade { get; set; }

    public string Descricao_Escolaridade { get; set; }

}