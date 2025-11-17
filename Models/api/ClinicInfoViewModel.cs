using Blood4A.Domain;

namespace Blood4A.Models;

public class ClinicaInfoViewModel()
{
    public required Clinicas Clinica { get; set; }
    public required AberturaFechamento[] Horarios { get; set; }
}