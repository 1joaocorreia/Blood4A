using Blood4A.Domain;
using Blood4A.Infrastructure;
using Blood4A.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blood4A.Controllers;

[Route("[controller]")]
public class HomeController(ApplicationDbContext db) : Controller
{
    private readonly ApplicationDbContext _db = db;

    [HttpGet]
    public IActionResult Index()
    {
        
        return View();

    }
    
    [HttpGet("clinic/{clinic_id}")]
    public async Task<IActionResult> Clinic(int clinic_id)
    {

        Clinicas? clinica = await _db.Clinicas
        .Include(clinica => clinica.obj_cep_location)
        .FirstOrDefaultAsync(clinica => clinica.id_clinica == clinic_id);

        if (clinica == null)
        {
            return NotFound( new { message = $"Clinic with id < {clinic_id} > not found" } );
        }

        AberturaFechamento[] horarios = await _db.AberturaFechamento
        .Where(horario => horario.referente_a == clinic_id)
        .ToArrayAsync();

        if (horarios.Length != 0)
        {
            var ordem = new Dictionary<string, int>
            {
                ["segunda feira"] = 1,
                ["terca feira"] = 2,
                ["terça feira"] = 2,
                ["quarta feira"] = 3,
                ["quinta feira"] = 4,
                ["sexta feira"] = 5,
            };

            horarios = horarios.OrderBy(
                x => ordem[x.dia_da_semana.Trim().ToLower()]
            ).ToArray();
        }

        return View(new ClinicaInfoViewModel(clinica, horarios));

    }
}