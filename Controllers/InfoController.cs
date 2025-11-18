using Blood4A.Infrastructure;
using Blood4A.Domain;
using Blood4A.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blood4A.Controllers;

[ApiController] 
[Route("api/info")]
public class InfoController(ApplicationDbContext db) : ControllerBase
{
    private readonly ApplicationDbContext _db = db;

    [HttpGet("clinic_data/{clinic_id}")]
    public async Task<IActionResult> GetClinicData(int clinic_id)
    {
        Clinicas? specific_clinic = await _db.Clinicas
            .Include(clinic => clinic.obj_cep_location)
            .FirstOrDefaultAsync(entity => entity.id_clinica == clinic_id);

        if (specific_clinic == null)
        {
            return NotFound(new { message = $"Clínica com ID {clinic_id} não encontrada." });
        }

        var horarios = await _db.AberturaFechamento
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
        
        return Ok(new ClinicaInfoViewModel() {
            Clinica = specific_clinic,
            Horarios = horarios.ToArray()
        });
    }

    [HttpGet("clinic_donations/{clinic_id}")]
    public async Task<IActionResult> GetClinicDonations(int clinic_id)
    {
        Clinicas? target_clinica = await _db.Clinicas.FirstOrDefaultAsync(clinic => clinic.id_clinica == clinic_id);
        if (target_clinica == null)
        {
            return NotFound(new { message = $"Nao foi possivel encontrar nenhuma clinica com o ID < {clinic_id} >" });
        }

        Doacoes[] doacoes = await _db.Doacoes
            .Where(entity => entity.id_clinica == clinic_id)
            .ToArrayAsync();

        if (doacoes.Length == 0)
        {
            return NotFound(new { message = "Não foi possivel localizar doações para a clínica especificada" });
        }

        Meses[] meses_do_ano = Enum.GetValues<Meses>(); // 1, 2, 3, 4, 5.... 12

        DoacaoMes[] doacoes_por_meses = new DoacaoMes[12];
        for (int i = 0; i < 12; i++)
        {
            doacoes_por_meses[i] = new DoacaoMes{
                Mes = meses_do_ano[i],
                QuantidadeDeDoacoes = 0
            };
        }

        foreach (var doacao in doacoes) {
            try {
                var mes = doacao.data_doacao.Split("/")[1]; // obtendo o mês da doação
                doacoes_por_meses[Int32.Parse(mes) - 1].QuantidadeDeDoacoes += 1; // aumentando a quantidade de doações
            } catch (Exception) {
                continue;
            }
        }

        return Ok( new ClinicaDonationsViewModel { DoacoesPorMes = doacoes_por_meses, ClinicaAlvo = target_clinica} );

    }

    [HttpGet("state_info/{estado}")]
    public async Task<IActionResult> GetStateInfo(string estado)
    {

        Clinicas[] clinicas = await _db.Clinicas
            .Include(clinica => clinica.obj_cep_location)
            .Where(clinica => clinica.obj_cep_location.estado.ToLower() == estado.ToLower())
            .ToArrayAsync();
        
        return Ok( new StateInfoViewModel() { Estado = estado, ListaDeClinicas = clinicas }  );

    }

    [HttpGet("state_donations/{estado}")]
    public async Task<IActionResult> GetStateDonations(string estado)
    {
        
        Doacoes[] doacoes = await _db.Doacoes
            .Include(doacao => doacao.obj_id_clinica)
            .Include(doacao => doacao.obj_id_clinica.obj_cep_location)
            .Where(doacao => doacao.obj_id_clinica.obj_cep_location.estado.ToLower() == estado.ToLower())
            .ToArrayAsync();
        
        if (doacoes.Length == 0)
        {
            return NotFound(new { message = $"Nenhuma doação encontrada para a clinica < {estado} >" });
        }

        Meses[] meses_do_ano = Enum.GetValues<Meses>();
        
        DoacaoMes[] doacoes_por_mes = new DoacaoMes[12];
        for (var i = 0; i < 12; i++)
        {
            doacoes_por_mes[i] = new DoacaoMes { Mes = meses_do_ano[i], QuantidadeDeDoacoes = 0 };
        }

        foreach (var doacao in doacoes)
        {
            try
            {
                var mes = doacao.data_doacao.Split("/")[1];
                doacoes_por_mes[Int32.Parse(mes) - 1].QuantidadeDeDoacoes+=1;
            } catch (Exception)
            {
                continue;
            }
        }

        return Ok(new StateDonationsViewModel { Estado = estado, DoacoesPorMes = doacoes_por_mes } );

    }

    [HttpGet("get_all_clinics/")]
    public async Task<IActionResult> GetAllClinics()
    {
        List<ClinicasEstado> clinicas_por_estado = new List<ClinicasEstado>();

        Clinicas[] all_clinics = await _db.Clinicas
            .Include(clinic => clinic.obj_cep_location)
            .ToArrayAsync();
        
        if (all_clinics.Length == 0)
        {
            return NotFound( new { message = "Nenhuma clinica encontrada" } );
        }
        
        for (int i = 0; i < all_clinics.Length; i++)
        {
            Clinicas current_clinic = all_clinics[i];
            if (current_clinic.obj_cep_location == null)
            {
                continue; // Não tem localização
            }

            // Checando se o código dessa clínica ja existe na lista
            ClinicasEstado? match = clinicas_por_estado.FirstOrDefault(c_estado => c_estado.Estado.Equals(current_clinic.obj_cep_location.estado, StringComparison.CurrentCultureIgnoreCase));
            if (match == null)
            {
                // Criar um novo ClinicaEstado
                ClinicasEstado created_clinica_estado = new ClinicasEstado { Estado = current_clinic.obj_cep_location.estado, ListaDeClinicas = new List<Clinicas>()};
                created_clinica_estado.ListaDeClinicas.Add(current_clinic);

                clinicas_por_estado.Add(created_clinica_estado);
                continue;
            } else
            {
                // Adicionar a clinica nesse ClinicaEstado
                match.ListaDeClinicas.Add(current_clinic);
                continue;
            }
        }

        return Ok( new AllClinicsViewModel() { ClinicasEstado = clinicas_por_estado.ToArray() } );

    }

}