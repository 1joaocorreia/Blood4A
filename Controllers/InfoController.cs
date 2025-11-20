using Blood4A.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blood4A.Services;
using Blood4A.Models;

namespace Blood4A.Controllers;

[ApiController] 
[Route("api/info")]
[Authorize]
public class InfoController(InformationService info) : ControllerBase
{
    private readonly InformationService _info = info;

    [HttpGet("clinic_donations/{clinic_id}")]
    public async Task<IActionResult> GetClinicDonations(int clinic_id)
    {
        ClinicaDonationsViewModel? model = await _info.GetClinicDonationsData(clinic_id);
        
        if (model == null)
        {
            return NotFound(new { message = $"Não foi possivel obter informações de doação desta clinica" });
        }

        return Ok(model);
    }

    [HttpGet("state_donations/{state}")]
    public async Task<IActionResult> GetStateDonations(string state)
    {
        StateDonationsViewModel? model = await _info.GetStateDonations(state);

        if (model == null)
        {
            return NotFound("Não foi possivel obter informações de doação deste estado");
        }

        return Ok(model);
    }

    [HttpGet("get_all_clinics")]
    public async Task<IActionResult> GetAllClinics()
    {
        AllClinicsViewModel? model = await _info.GetAllClinics();

        if (model == null)
        {
            return NotFound("Nenhuma clinica encontrada");
        }

        return Ok(model);
    }

}