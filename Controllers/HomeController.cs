using System.Text.Json;
using Blood4A.Infrastructure;
using Blood4A.Models;
using Blood4A.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blood4A.Controllers;

[Route("home")]
[Authorize]
public class HomeController(InformationService info) : Controller
{
    private readonly InformationService _info = info;

    [HttpGet]
    public IActionResult Index()
    {   
        return View();
    }
    
    [HttpGet("clinic/{clinic_id}")]
    public async Task<IActionResult> Clinic(int clinic_id)
    {
        ClinicaInfoViewModel? model = await _info.GetClinicData(clinic_id);

        if (model == null)
        {
            return (IActionResult)Results.InternalServerError( new { message = "Não foi possivel deserializar a resposta obtida do servidor." } );
        }

        return View(model);
    }

    [HttpGet("state/{estado}")]
    public async Task<IActionResult> State(string estado)
    {
        StateInfoViewModel? model = await _info.GetStateInfo(estado);

        if (model == null)
        {
            return (IActionResult)Results.InternalServerError( new { message = "Não foi possivel deserializar a resposta obtida do servidor." } );
        }

        return View(model);
    }
}