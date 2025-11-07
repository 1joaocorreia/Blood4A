namespace Blood4A.Models;

public class Localizacoes
{
    [System.ComponentModel.DataAnnotations.Key]
    public string Cep { get; set; }

    public string Logradouro { get; set; }

    public string Bairro { get; set; }

    public string Cidade { get; set; }

    public string Estado { get; set; }
    
    public int Uf { get; set; }

}
