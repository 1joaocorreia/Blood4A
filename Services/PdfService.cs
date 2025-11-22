using Blood4A.Models;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Blood4A.Services;

public interface IPdfService<T>
{
    public Task<byte[]> GeneratePdf(T param);
};


public class PdfClinicService(InformationService info) : IPdfService<int>
{

    private readonly InformationService _info = info;

    public async Task<byte[]> GeneratePdf(int clinic_id)
    {

        ClinicaInfoViewModel? clinica_info = await _info.GetClinicData(clinic_id);
        ClinicaDonationsViewModel? clinica_donations = await _info.GetClinicDonationsData(clinic_id);
        
        if (clinica_info == null)
        {
            return [];
        }

        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdf = new PdfDocument(writer);
        var doc = new Document(pdf);

        doc.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

        doc.Add(new Paragraph("Relatório da Instituição").SetFontSize(18));
        
        doc.Add(new Paragraph($"NOME: {clinica_info.Clinica.nome_clinica}"));
        doc.Add(new Paragraph($"CNPJ: {clinica_info.Clinica.cnpj_clinica}"));
        if (clinica_info.Clinica.obj_cep_location != null)
        {
            doc.Add(new Paragraph("Localização da Instituição")
                .SetFontSize(18));
            doc.Add(new Paragraph($"CEP: {clinica_info.Clinica.obj_cep_location.cep}"));
            doc.Add(new Paragraph($"LOGRADOURO: {clinica_info.Clinica.obj_cep_location.logradouro}"));
            doc.Add(new Paragraph($"BAIRRO: {clinica_info.Clinica.obj_cep_location.bairro}"));
            doc.Add(new Paragraph($"CIDADE: {clinica_info.Clinica.obj_cep_location.cidade}"));
            doc.Add(new Paragraph($"ESTADO: {clinica_info.Clinica.obj_cep_location.estado}"));
            doc.Add(new Paragraph($"UF: {clinica_info.Clinica.obj_cep_location.uf}"));
        }

        
        if (clinica_donations == null)
        {
            doc.Add(new Paragraph("Não foi possivel carregar os dados de doações desta clínica especifica "));
        } else
        {            
            doc.Add(new Paragraph("Quantidade Mensal de Doações").SetFontSize(18));

            foreach (var doacao_mes in clinica_donations.DoacoesPorMes)
            {
                doc.Add(new Paragraph($"Mês de {doacao_mes.Mes}: {doacao_mes.QuantidadeDeDoacoes}"));
            }

            byte[] generated_chart = new MonthlyDonationsChartService().GenerateMemoryPngFor(clinica_donations);
            doc.Add(new Image(ImageDataFactory.CreatePng(generated_chart)));

            doc.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_PAGE));
        }
        

        doc.Add(new Paragraph("\n--- Fim do relatório ---"));

        doc.Close();
        
        return ms.ToArray();
    }
}

public class PdfStateService(InformationService info) : IPdfService<string>
{

    private readonly InformationService _info = info;

    public async Task<byte[]> GeneratePdf(string state)
    {
        StateInfoViewModel? state_info = await _info.GetStateInfo(state);
        if (state_info == null)
        {
            return [];
        }
        
        StateDonationsViewModel? state_donations = await _info.GetStateDonations(state);
        if (state_donations == null)
        {
            return [];
        }

        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdf = new PdfDocument(writer);
        var doc = new Document(pdf);

        doc.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

        doc.Add(new Paragraph("Relatório sobre Estado").SetFontSize(18));

        doc.Add(new Paragraph($"{state_donations.Estado}"));
        
        doc.Add(new Paragraph("Quantidade de Doações Mensais").SetFontSize(18));
        
        foreach (var doacao_mes in state_donations.DoacoesPorMes)
        {
            doc.Add(new Paragraph($"Mes de {doacao_mes.Mes}: {doacao_mes.QuantidadeDeDoacoes} doações"));
        }

        byte[] generated_chart = new MonthlyDonationsChartService().GenerateMemoryPngFor(state_donations);

        doc.Add(new Image(ImageDataFactory.CreatePng(generated_chart)));

        foreach (var clinica in state_info.ListaDeClinicas)
        {
            doc.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_PAGE));
            doc.Add(new Paragraph($"Clinica {clinica.nome_clinica}").SetFontSize(18));
            doc.Add(new Paragraph($"CNPJ: {clinica.cnpj_clinica}"));
            if (clinica.obj_cep_location != null) {
                doc.Add(new Paragraph($"Localização").SetFontSize(18));
                doc.Add(new Paragraph($"CEP: {clinica.obj_cep_location.cep}"));
                doc.Add(new Paragraph($"LOGRADOURO: {clinica.obj_cep_location.logradouro}"));
                doc.Add(new Paragraph($"BAIRRO: {clinica.obj_cep_location.bairro}"));
                doc.Add(new Paragraph($"CIDADE: {clinica.obj_cep_location.cidade}"));
                doc.Add(new Paragraph($"ESTADO: {clinica.obj_cep_location.estado}"));
                doc.Add(new Paragraph($"UF: {clinica.obj_cep_location.uf}"));
            }

            ClinicaDonationsViewModel? clinica_donations = await _info.GetClinicDonationsData(clinica.id_clinica);
            if (clinica_donations != null)
            {
                doc.Add(new Paragraph("Quantidade Mensal de Doações").SetFontSize(18));
                foreach (var doacao_mes in clinica_donations.DoacoesPorMes)
                {
                    doc.Add(new Paragraph($"Mês de {doacao_mes.Mes}: {doacao_mes.QuantidadeDeDoacoes}"));
                }
                generated_chart = new MonthlyDonationsChartService().GenerateMemoryPngFor(clinica_donations);
                doc.Add(new Image(ImageDataFactory.CreatePng(generated_chart)));
            }
            
        }
        
        doc.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_PAGE));

        doc.Add(new Paragraph("\n--- Fim do relatório ---"));

        doc.Close();

        return ms.ToArray();
    }
}
