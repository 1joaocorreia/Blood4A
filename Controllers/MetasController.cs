using Blood4A.Domain;
using Blood4A.Infrastructure;
using Blood4A.Services;
using Microsoft.AspNetCore.Mvc;

[Route("metas")]
public class MetasController(ApplicationDbContext db, InformationService info) : Controller
{
    private readonly ApplicationDbContext _db = db;
    private readonly InformationService _info = info;

    [HttpGet("clinic/{clinic_id}")]
    public IActionResult Clinic(int clinic_id)
    {
        Clinicas? match = _db.Clinicas
            .FirstOrDefault(clinica => clinica.id_clinica == clinic_id);

        if (match == null)
        {
            return NotFound(new { message = "Nenhuma clinica encontrada" });
        }

        return View(match);   
    }
}