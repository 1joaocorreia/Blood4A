using Blood4A.Domain;
using Blood4A.Infrastructure;
using Blood4A.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blood4A.Controllers;

[ApiController]
[Route("api/export")]
[Authorize]
public class ExportController(InformationService info) : ControllerBase
{
    private readonly InformationService _info = info;

    [HttpGet("clinic/{clinic_id}")]
    public async Task<IActionResult> ClinicReport(int clinic_id)
    {
        byte[] pdf_bytes = await new PdfClinicService(_info).GeneratePdf(clinic_id);
        if (pdf_bytes.Length == 0)
        {
            return (IActionResult) Results.InternalServerError(new { message = "Nao foi possivel gerar o pdf para a clinica especificada" });
        }

        return File(pdf_bytes, "application/pdf", $"clinic_{clinic_id}.pdf");
    }

    [HttpGet("state/{state}")]
    public async Task<IActionResult> StateReport(string state)
    {
        byte[] pdf_bytes = await new PdfStateService(_info).GeneratePdf(state);
        if (pdf_bytes.Length == 0)
        {
            return (IActionResult) Results.InternalServerError(new { message = "Nao foi possivel gerar o pdf para o estado especificado" });
        }

        return File(pdf_bytes, "application/pdf", $"state_{state.ToLower().Replace(' ', '_')}.pdf");
    }

}