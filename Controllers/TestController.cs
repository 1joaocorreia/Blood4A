using Blood4A.Infrastructure;
using Blood4A.Domain;
using Blood4A.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blood4A.Controllers;

[Route("[controller]")]
public class TestController(ApplicationDbContext db) : Controller
{
    private ApplicationDbContext _db = db;

    [HttpGet]
    public IActionResult Database()
    {
        Doacoes? primeira_doacao = _db.Doacoes
        .Include(d => d.obj_id_clinica)
        .Include(d => d.obj_id_clinica.obj_cep_location)
        .Include(d => d.obj_id_doador)
        .Include(d => d.obj_id_doador.obj_id_escolaridade)
        .Include(d => d.obj_id_agente)
        .First<Doacoes>();
        if (primeira_doacao == null)
        {
            return NotFound(new { message = "Nenhuma doação encontrada" });
        }

        DatabaseViewModel model = new DatabaseViewModel { Doacao = primeira_doacao };
        
        return View(model);
    }

}