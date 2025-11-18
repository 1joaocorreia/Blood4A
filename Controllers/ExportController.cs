using Blood4A.Domain;
using Blood4A.Infrastructure;
using Blood4A.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blood4A.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExportController(ApplicationDbContext db) : ControllerBase
{
    private readonly ApplicationDbContext _db = db;

    [HttpGet("clinic/{clinic_id}")]
    public async Task<IActionResult> ClinicReport(int clinic_id)
    {

        Clinicas? clinica = _db.Clinicas
            .FirstOrDefault(clinica => clinica.id_clinica == clinic_id);
        
        if (clinica == null)
        {
            return NotFound( new { message = "Clinica nao encontrada" } );
        }

        byte[] pdf_bytes = await new PdfClinicService().GeneratePdf(clinica);
        if (pdf_bytes.Length == 0)
        {
            return (IActionResult) Results.InternalServerError(new { message = "Nao foi possivel gerar o pdf para a clinica especificada" });
        }

        return File(pdf_bytes, "application/pdf", $"clinic_{clinic_id}.pdf");

    }

    [HttpGet("state/{state}")]
    public async Task<IActionResult> StateReport(string state)
    {
        
        byte[] pdf_bytes = await new PdfStateService().GeneratePdf(state);
        if (pdf_bytes.Length == 0)
        {
            return (IActionResult) Results.InternalServerError(new { message = "Nao foi possivel gerar o pdf para o estado especificado" });
        }

        return File(pdf_bytes, "application/pdf", $"state_{state.ToLower().Replace(' ', '_')}.pdf");

    }

}