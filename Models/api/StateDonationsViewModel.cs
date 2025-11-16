using Blood4A.Models;

public class StateDonationsViewModel (string Estado, DoacaoMes[] DoacoesPorMes)
{
    public string Estado {get; set;} = Estado;
    public DoacaoMes[] DoacoesPorMes {get; set;} = DoacoesPorMes;
}