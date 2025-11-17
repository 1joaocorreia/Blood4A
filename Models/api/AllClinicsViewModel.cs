using Blood4A.Domain;

namespace Blood4A.Models;

public class ClinicasEstado()
{
    public required string Estado { get; set; }
    public string? EstadoCode { get; set; }
    public required List<Clinicas> ListaDeClinicas { get; set; }
}

public class AllClinicsViewModel()
{
    public required ClinicasEstado[] ClinicasEstado { get; set; }
}