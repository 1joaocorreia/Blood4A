using Blood4A.Data;
using Blood4A.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blood4A.Controllers;

public class TestController : Controller
{

    private ApplicationDbContext _db;

    public TestController(ApplicationDbContext db)
    {
        _db = db;
    }

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
            throw new OperationCanceledException("Nenhuma doação encontrada");
        }

        DatabaseViewModel model = new DatabaseViewModel(primeira_doacao);
        
        return View(model);
    }

}