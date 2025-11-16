using Microsoft.AspNetCore.Mvc;

using Blood4A.Data;

namespace Blood4A.Controllers
{
    public class ClinicasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClinicasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clinicas = _context.Clinicas.ToList();
            return View(clinicas);
        }
    }
}