using Microsoft.AspNetCore.Mvc;
using Blood4A.Infrastructure;

namespace Blood4A.Controllers;

public class ClinicasController : Controller
{
    private readonly ApplicationDbContext _db;

    public ClinicasController(ApplicationDbContext context)
    {
        _db = context;
    }

    public IActionResult Index()
    {
        var clinicas = _db.Clinicas.ToList();
        return View(clinicas);
    }
}
