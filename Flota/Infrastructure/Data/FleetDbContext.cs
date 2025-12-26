using Microsoft.EntityFrameworkCore;
using Flota.Domain.Entities;

namespace Flota.Infrastructure.Data;

public class FleetDbContext : DbContext 
{
    public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options) { }

    // Zostawiamy tylko główną tabelę Pojazdy (bez podziału na typy)
    public DbSet<Pojazd> Pojazdy { get; set; }
    public DbSet<Kierowca> Kierowcy { get; set; }
    public DbSet<Tankowanie> Tankowania { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        // Usunąłem konfigurację "HasDiscriminator", bo już jej nie potrzebujemy.
        // Zostawiamy tylko unikalność numeru rejestracyjnego.
        modelBuilder.Entity<Pojazd>()
            .HasIndex(p => p.NumerRejestracyjny)
            .IsUnique();
    }
}