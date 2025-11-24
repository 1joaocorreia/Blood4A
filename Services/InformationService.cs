using Blood4A.Domain;
using Blood4A.Infrastructure;
using Blood4A.Models;
using Microsoft.EntityFrameworkCore;

namespace Blood4A.Services;

public class InformationService
{
    private readonly ApplicationDbContext _db;

    public InformationService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ClinicaInfoViewModel?> GetClinicData(int clinic_id)
    {
        Clinicas? specific_clinic = await _db.Clinicas
            .Include(clinic => clinic.obj_cep_location)
            .FirstOrDefaultAsync(entity => entity.id_clinica == clinic_id);

        if (specific_clinic == null)
        {
            return null;
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
        
        return new ClinicaInfoViewModel() {
            Clinica = specific_clinic,
            Horarios = horarios.ToArray()
        };
    }

    public async Task<ClinicaDonationsViewModel?> GetClinicDonationsData(int clinic_id)
    {
        Clinicas? target_clinica = await _db.Clinicas.FirstOrDefaultAsync(clinic => clinic.id_clinica == clinic_id);
        if (target_clinica == null)
        {
            return null;
        }

        Doacoes[] doacoes = await _db.Doacoes
            .Where(entity => entity.id_clinica == clinic_id)
            .ToArrayAsync();

        if (doacoes.Length == 0)
        {
            return new ClinicaDonationsViewModel() { ClinicaAlvo = target_clinica, DoacoesPorMes = [] };
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

        return new ClinicaDonationsViewModel { DoacoesPorMes = doacoes_por_meses, ClinicaAlvo = target_clinica};

    }

    public async Task<MetasClinica[]> GetClinicGoals(int clinic_id)
    {
        MetasClinica[] goals = await _db.MetasClinica
            .Where(goal => goal.referente_a == clinic_id)
            .ToArrayAsync();

        return goals;
    }
    public async Task<StateInfoViewModel> GetStateInfo(string estado)
    {
        Clinicas[] clinicas = await _db.Clinicas
            .Include(clinica => clinica.obj_cep_location)
            .Where(clinica => clinica.obj_cep_location.estado.ToLower() == estado.ToLower())
            .ToArrayAsync();
        
        return new StateInfoViewModel() { Estado = estado, ListaDeClinicas = clinicas };

    }

    public async Task<StateDonationsViewModel?> GetStateDonations(string estado)
    {
        Doacoes[] doacoes = await _db.Doacoes
            .Include(doacao => doacao.obj_id_clinica)
            .Include(doacao => doacao.obj_id_clinica.obj_cep_location)
            .Where(doacao => doacao.obj_id_clinica.obj_cep_location.estado.ToLower() == estado.ToLower())
            .ToArrayAsync();
        
        if (doacoes.Length == 0)
        {
            return new StateDonationsViewModel() { Estado = estado, DoacoesPorMes = [] };
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

        return new StateDonationsViewModel { Estado = estado, DoacoesPorMes = doacoes_por_mes };
    }

    public async Task<MetasEstado[]> GetStateGoals(string estado)
    {
        MetasEstado[] goals = await _db.MetasEstado
            .Where(goal => goal.referente_a.ToLower() == estado.ToLower())
            .ToArrayAsync();

        return goals;
    }

    public async Task<AllClinicsViewModel?> GetAllClinics()
    {
        List<ClinicasEstado> clinicas_por_estado = new List<ClinicasEstado>();

        Clinicas[] all_clinics = await _db.Clinicas
            .Include(clinic => clinic.obj_cep_location)
            .ToArrayAsync();
        
        if (all_clinics.Length == 0)
        {
            return null;
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

        return new AllClinicsViewModel() { ClinicasEstado = clinicas_por_estado.ToArray() };

    }
}