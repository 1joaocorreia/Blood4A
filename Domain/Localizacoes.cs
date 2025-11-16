namespace Blood4A.Domain;

public class Localizacoes
{
    [System.ComponentModel.DataAnnotations.Key]
    public required string cep { get; set; }

    public required string logradouro { get; set; }

    public required string bairro { get; set; }

    public required string cidade { get; set; }

    public required string estado { get; set; }
    
    public required int uf { get; set; }

}
