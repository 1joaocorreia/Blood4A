using Blood4A.Data;

namespace Blood4A.Models;

public class DatabaseViewModel
{

    public Doacoes doacao;

    public DatabaseViewModel(Doacoes doacao)
    {
        this.doacao = doacao;
    }

}