using ScottPlot;

public interface IChartService<T>
{
    public byte[] GenerateMemoryPngFor(T model);
}

public class MonthlyDonationsChartService(): IChartService<DonationsViewModel>
{
    public byte[] GenerateMemoryPngFor(DonationsViewModel donations)
    {
        if (donations == null || donations.DoacoesPorMes.Length == 0)
        {
            return [];
        }

        Plot plot = new(); 

		foreach (var donation_month in donations.DoacoesPorMes)
        {
            Bar bar_to_add = new Bar();
            bar_to_add.Position = (double) (donation_month.Mes - 1);
            bar_to_add.Value = donation_month.QuantidadeDeDoacoes;
            plot.Add.Bar(bar_to_add);
        }
		
		Tick[] ticks =
        {
            new (0, "Jan"),
			new (1, "Fev"),
			new (2, "Mar"),
			new (3, "Abr"),
			new (4, "Mai"),
			new (5, "Jun"),
			new (6, "Jul"),
			new (7, "Ago"),
			new (8, "Set"),
			new (9, "Out"),
			new (10, "Nov"),
			new (11, "Dez"),
        };

		plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
		plot.Axes.Bottom.MajorTickStyle.Length = 0;
		plot.HideGrid();

		plot.Axes.Margins(bottom: 0);

		return plot.GetImage(800, 600).GetImageBytes(ImageFormat.Png);
    }
}
