using Blood4A.Domain;

namespace Blood4A.Models;

public class ClinicaInfoViewModel(Clinicas Clinica, AberturaFechamento[] Horarios)
{
    public Clinicas Clinica { get; set; } = Clinica;
    public AberturaFechamento[] Horarios { get; set; } = Horarios;
}