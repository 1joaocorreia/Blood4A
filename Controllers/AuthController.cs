using Microsoft.AspNetCore.Mvc;
using Blood4A.Models;
using System.Diagnostics;

namespace Blood4A.Controllers;

[Route("[controller]")]
public class AuthController(ILogger<AuthController> logger) : Controller
{
    private readonly ILogger<AuthController> _logger = logger;

    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Login", "Auth");
    }

    [HttpGet("Login/")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("Login/")]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Login.Equals("admin@hotmail.com", StringComparison.CurrentCultureIgnoreCase) && model.Password.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
            {
                return RedirectToAction("Index", "Home");
            }
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}