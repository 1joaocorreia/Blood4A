using Blood4A.Domain;

public class StateInfoViewModel (string Estado, Clinicas[] ListaDeClinicas)
{
    public string Estado {get; set;} = Estado;
    public Clinicas[] ListaDeClinicas {get; set;} = ListaDeClinicas;

}