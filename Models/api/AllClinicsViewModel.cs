using Blood4A.Domain;

namespace Blood4A.Models;

public class ClinicasEstado(string Estado, string EstadoCode)
{
    public string Estado { get; set; } = Estado;
    public string EstadoCode { get; set; } = EstadoCode;
    public List<Clinicas> ListaDeClinicas { get; set; } = [];
}

public class AllClinicsViewModel(ClinicasEstado[] ClinicasEstados)
{
    public ClinicasEstado[] ClinicasEstado { get; set; } = ClinicasEstados;
}