using Microsoft.AspNetCore.Mvc;
using Blood4A.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Blood4A.Controllers;

[Route("auth")]
public class AuthController(ILogger<AuthController> logger) : Controller
{
    private readonly ILogger<AuthController> _logger = logger;

    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("login", "auth");
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("index", "home");
        }
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Login.Equals("admin@hotmail.com", StringComparison.CurrentCultureIgnoreCase) && model.Password.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
            {
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Login),
                    new Claim(ClaimTypes.Role, "User")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme
                );

                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                return RedirectToAction("index", "home");

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