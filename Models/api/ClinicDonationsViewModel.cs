using Blood4A.Domain;

namespace Blood4A.Models;

public class ClinicaDonationsViewModel : DonationsViewModel
{
    public required Clinicas ClinicaAlvo { get; set; }
}