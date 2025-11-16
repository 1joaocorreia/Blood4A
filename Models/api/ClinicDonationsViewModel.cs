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

public class DoacaoMes(Meses Mes, int QuantidadeDeDoacoes)
{
    public Meses Mes { get; set; } = Mes;
    public int QuantidadeDeDoacoes { get; set; } = QuantidadeDeDoacoes;
}

public class ClinicaDonationsViewModel(DoacaoMes[] DoacoesPorMes, Clinicas ClinicaAlvo)
{

    public DoacaoMes[] DoacoesPorMes { get; set; } = DoacoesPorMes;
    public Clinicas ClinicaAlvo { get; set; } = ClinicaAlvo;
}