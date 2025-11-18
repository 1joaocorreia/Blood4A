using Blood4A.Domain;
using Blood4A.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Text.Json;

namespace Blood4A.Services;

public interface IPdfService<T>
{
    public Task<byte[]> GeneratePdf(T param);
};


public class PdfClinicService : IPdfService<Clinicas>
{

    private readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5213") };
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public async Task<byte[]> GeneratePdf(Clinicas Clinica)
    {

        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdf = new PdfDocument(writer);
        var doc = new Document(pdf);

        doc.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

        doc.Add(new Paragraph("Relatório da Instituição").SetFontSize(18));
        
        doc.Add(new Paragraph($"NOME: {Clinica.nome_clinica}"));
        doc.Add(new Paragraph($"CNPJ: {Clinica.cnpj_clinica}"));
        if (Clinica.obj_cep_location != null)
        {
            doc.Add(new Paragraph("Localização da Instituição")
                .SetFontSize(18));
            doc.Add(new Paragraph($"CEP: {Clinica.obj_cep_location.cep}"));
            doc.Add(new Paragraph($"LOGRADOURO: {Clinica.obj_cep_location.logradouro}"));
            doc.Add(new Paragraph($"BAIRRO: {Clinica.obj_cep_location.bairro}"));
            doc.Add(new Paragraph($"CIDADE: {Clinica.obj_cep_location.cidade}"));
            doc.Add(new Paragraph($"ESTADO: {Clinica.obj_cep_location.estado}"));
            doc.Add(new Paragraph($"UF: {Clinica.obj_cep_location.uf}"));
        }

        var donations_response = await client.GetAsync($"/api/info/clinic_donations/{Clinica.id_clinica}");
        if (donations_response.IsSuccessStatusCode)
        {
            var text_data = await donations_response.Content.ReadAsStringAsync();
            ClinicaDonationsViewModel? json_data = JsonSerializer.Deserialize<ClinicaDonationsViewModel>(text_data, jsonOptions);
            if (json_data == null)
            {
                doc.Add(new Paragraph("Não foi possivel carregar os dados de doações desta clínica especifica "));
            } else
            {
                doc.Add(new Paragraph("Numero de Doações Mensais: ").SetFontSize(18));
                
                foreach (var doacao_por_mes in json_data.DoacoesPorMes)
                {
                    doc.Add(new Paragraph($"Mês de {doacao_por_mes.Mes}: {doacao_por_mes.QuantidadeDeDoacoes} doações"));
                }
            }
        }

        doc.Add(new Paragraph("\n--- Fim do relatório ---"));

        doc.Close();
        
        return ms.ToArray();
    }
}

public class PdfStateService : IPdfService<string>
{
    private readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5213") };
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    public async Task<byte[]> GeneratePdf(string estado)
    {
        var response = await client.GetAsync($"/api/info/state_donations/{estado}");
        if (! response.IsSuccessStatusCode)
        {
            return [];
        }

        StateDonationsViewModel? state_donations_json_data = JsonSerializer.Deserialize<StateDonationsViewModel>(await response.Content.ReadAsStringAsync(), jsonOptions);
        if (state_donations_json_data == null)
        {
            return [];
        }

        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdf = new PdfDocument(writer);
        var doc = new Document(pdf);

        doc.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

        doc.Add(new Paragraph("Relatório sobre Estado").SetFontSize(18));

        doc.Add(new Paragraph($"NOME: {state_donations_json_data.Estado}"));
        
        doc.Add(new Paragraph("Quantidade de Doações Mensais").SetFontSize(18));
        
        foreach (var doacao_mes in state_donations_json_data.DoacoesPorMes)
        {
            doc.Add(new Paragraph($"Mes de {doacao_mes.Mes}: {doacao_mes.QuantidadeDeDoacoes} doações"));
        }

        response = await client.GetAsync($"/api/info/state_info/{estado}");
        if (response.IsSuccessStatusCode) {
            StateInfoViewModel? state_info_json_data = JsonSerializer.Deserialize<StateInfoViewModel>(await response.Content.ReadAsStringAsync(), jsonOptions);
            if (state_info_json_data != null && state_info_json_data.ListaDeClinicas.Length > 0)
            {
                foreach (var clinica in state_info_json_data.ListaDeClinicas)
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

                    response = await client.GetAsync($"/api/info/clinic_donations/{clinica.id_clinica}");
                    if (response.IsSuccessStatusCode)
                    {
                        ClinicaDonationsViewModel? clinica_donations_json_data = JsonSerializer.Deserialize<ClinicaDonationsViewModel>(await response.Content.ReadAsStringAsync(), jsonOptions);
                        if (clinica_donations_json_data != null)
                        {
                            doc.Add(new Paragraph("Quantidade de Doações").SetFontSize(18));
                            foreach (var doacao_mes in clinica_donations_json_data.DoacoesPorMes)
                            {
                                doc.Add(new Paragraph($"Mês de {doacao_mes.Mes}: {doacao_mes.QuantidadeDeDoacoes}"));
                            }
                        }
                    }
                }
            }
        }

        doc.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.LAST_PAGE));

        doc.Add(new Paragraph("\n--- Fim do relatório ---"));

        doc.Close();

        return ms.ToArray();
    }
}
