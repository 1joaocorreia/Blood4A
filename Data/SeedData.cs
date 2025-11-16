using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Blood4A.Models; // ajuste conforme o namespace dos seus modelos
//using Blood4.Models; // ou o namespace exato onde está a classe Clinicas



namespace Blood4A.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Evita duplicação
            if (context.Clinicas.Any() || context.AberturaFechamento.Any() || context.Agentes.Any() || context.Doacoes.Any() || context.Doadores.Any() || context.Escolaridade.Any()) return;

             var escolaridade1 = new Escolaridade {
    Grau_Escolaridade = "Ensino Fundamental",
    Descricao_Escolaridade = "Concluído o ensino fundamental."
};
             var escolaridade2 = new Escolaridade {
    Grau_Escolaridade = "Ensino Médio",
    Descricao_Escolaridade = "Concluído o ensino médio."
};
             var escolaridade3 = new Escolaridade {
    Grau_Escolaridade = "Ensino Superior",
    Descricao_Escolaridade = "Concluído o ensino superior."
};
            context.Escolaridade.AddRange(escolaridade1, escolaridade2, escolaridade3);
            context.SaveChanges();

             var doadores = new Doadores[]
{
    new Doadores {
        Nome_Doador = "Carlos Pereira",
        Data_Nascimento_Doador = "1990-05-20",
        Cpf_Doador = "123.456.789-00",
        Telefone_Doador = "(11) 91234-5678",
        Tipo_Sanguineo_Doador = "O+",
        cep_location = "06000-000",
        id_escolaridade = escolaridade1.Id_Escolaridade
    },
    new Doadores {
        Nome_Doador = "Ana Souza",
        Data_Nascimento_Doador = "1985-08-15",
        Cpf_Doador = "987.654.321-00",
        Telefone_Doador = "(21) 99876-5432",
        Tipo_Sanguineo_Doador = "A-",
        cep_location = "07000-000",
        id_escolaridade = escolaridade2.Id_Escolaridade
    }
};
            context.Doadores.AddRange(doadores);
            context.SaveChanges();

            var clinicas = new Clinicas[]
            {
                new Clinicas {
                    Nome_Clinica = "Clínica Vida",
                    Cnpj_Clinica = "12.345.678/0001-90",
                    cep_location = "04000-000"
                },

                new Clinicas {
                    Nome_Clinica = "Clínica Saúde",
                    Cnpj_Clinica = "98.765.432/0001-10",
                    cep_location = "05000-000"
                }
            };
            context.Clinicas.AddRange(clinicas);
            context.SaveChanges();

            var aberturaFechamentos = new AberturaFechamento[]
            {
                new AberturaFechamento {
                    referente_a = clinicas[0].Id_Clinica, // FK para Clínica Vida
                    Horario_Abertura = "08:00",
                    Horario_Fechamento = "18:00",
                    Dia_Da_Semana = "Segunda a Sexta"
                },
                new AberturaFechamento {
                    referente_a = clinicas[0].Id_Clinica, // FK para Clínica Vida
                    Horario_Abertura = "09:00",
                    Horario_Fechamento = "14:00",
                    Dia_Da_Semana = "Sábado"
                }
            };
            context.AberturaFechamento.AddRange(aberturaFechamentos);
            context.SaveChanges();

            var agentes = new Agentes[]
            {
                new Agentes {
                    Nome_Agente = "João Silva",
                    Cnpj_Agente = "98.765.432/0001-10",
                    Email_Agente = "joaosilva@gmail.com",
                    Senha_Agente = "senha123"
                },

                new Agentes {
                    Nome_Agente = "Maria Oliveira",
                    Cnpj_Agente = "87.654.321/0001-20",
                    Email_Agente = "mariaoliveira@gmail.com",
                    Senha_Agente = "senha456"
                }
            };
            context.Agentes.AddRange(agentes);
            context.SaveChanges();

            var doacoes = new Doacoes[]
            {
                new Doacoes {
                    id_agente = agentes[0].Id_Agente, // FK para João Silva
                    id_doador = 1, // Supondo que o doador com Id 1 já exista
                    id_clinica = clinicas[0].Id_Clinica, // FK para Clínica Vida
                    Data_Doacao = "2024-01-15",
                    Hora_Doacao = "10:30"
                },
                new Doacoes {
                    id_agente = agentes[1].Id_Agente, // FK para Maria Oliveira
                    id_doador = 2, // Supondo que o doador com Id 2 já exista
                    id_clinica = clinicas[0].Id_Clinica, // FK para Clínica Vida
                    Data_Doacao = "2024-01-16",
                    Hora_Doacao = "11:00"
                }
            };
            context.Doacoes.AddRange(doacoes);
            context.SaveChanges();


        }
    }
}