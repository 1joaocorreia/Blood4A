using Blood4A.Domain;

namespace Blood4A.Models;

public enum Meses
{
    Janeiro = 1,
    Fevereiro,
    Marco,
    Abril,
    Maio,
    Junho,
    Julho,
    Agosto,
    Setembro,
    Outubro,
    Novembro,
    Dezembro
}

public class DoacaoMes()
{
    public Meses Mes { get; set; }
    public int QuantidadeDeDoacoes { get; set; }
}

public class ClinicaDonationsViewModel()
{

    public required DoacaoMes[] DoacoesPorMes { get; set; }
    public required Clinicas ClinicaAlvo { get; set; }
}