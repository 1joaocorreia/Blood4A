using System.Text.Json;
using Blood4A.Infrastructure;
using Blood4A.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blood4A.Controllers;

[Route("[controller]")]
public class HomeController(ApplicationDbContext db) : Controller
{
    private readonly ApplicationDbContext _db = db;
    private readonly HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:5213") };
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    [HttpGet]
    public IActionResult Index()
    {
        
        return View();

    }
    
    [HttpGet("clinic/{clinic_id}")]
    public async Task<IActionResult> Clinic(int clinic_id)
    {

        var response = await client.GetAsync($"/api/info/clinic_data/{clinic_id}");
        if (! response.IsSuccessStatusCode)
        {
            return (IActionResult)Results.InternalServerError(new { message =  "Não foi possivel obter os dados da clinica especificada"});
        }

        ClinicaInfoViewModel? model = JsonSerializer.Deserialize<ClinicaInfoViewModel>(
            await response.Content.ReadAsStringAsync(),
            jsonOptions
        );

        if (model == null)
        {
            return (IActionResult)Results.InternalServerError( new { message = "Não foi possivel deserializar a resposta obtida do servidor." } );
        }

        return View(model);

    }

    [HttpGet("state/{estado}")]
    public async Task<IActionResult> State(string estado)
    {
        
        var response = await client.GetAsync($"/api/info/state_info/{estado}");
        if (! response.IsSuccessStatusCode)
        {
            return (IActionResult)Results.InternalServerError(new { message =  "Não foi possivel obter informações do estado especificado"});
        }

        StateInfoViewModel? model = JsonSerializer.Deserialize<StateInfoViewModel>(
            await response.Content.ReadAsStringAsync(),
            jsonOptions
        );

        if (model == null)
        {
            return (IActionResult)Results.InternalServerError( new { message = "Não foi possivel deserializar a resposta obtida do servidor." } );
        }

        return View(model);

    }
}