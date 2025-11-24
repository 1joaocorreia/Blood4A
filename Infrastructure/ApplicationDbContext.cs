using Microsoft.EntityFrameworkCore;
using Blood4A.Domain;


namespace Blood4A.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AberturaFechamento>().HasKey(table => new
        {
            table.referente_a, table.dia_da_semana
        });
    }

    public DbSet<AberturaFechamento> AberturaFechamento { get; set; }
    public DbSet<Agentes> Agentes { get; set; }
    public DbSet<Clinicas> Clinicas { get; set; }
    public DbSet<Doacoes> Doacoes { get; set; }
    public DbSet<Doadores> Doadores { get; set; }
    public DbSet<Escolaridade> Escolaridade { get; set; }
    public DbSet<Localizacoes> Localizacoes { get; set; }
    public DbSet<MetasClinica> MetasClinica {get; set;}
    public DbSet<MetasEstado> MetasEstado {get; set;}

}