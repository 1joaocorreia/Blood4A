namespace Blood4A.Domain;

public class Escolaridade
{
    [System.ComponentModel.DataAnnotations.Key]
    public int id_escolaridade { get; set; }

    public required string grau_escolaridade { get; set; }

    public required string descricao_escolaridade { get; set; }

}