using Blood4A.Domain;

namespace Blood4A.Models;

public class DatabaseViewModel(Doacoes Doacao)
{
    public Doacoes Doacao { get; set; } = Doacao;
}