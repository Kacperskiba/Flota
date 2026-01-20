using Microsoft.EntityFrameworkCore;
using Flota.Domain.Entities;

namespace Flota.Infrastructure.Data;

public class FleetDbContext : DbContext 
{
    public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options) { }

    public DbSet<Pojazd> Pojazdy { get; set; }
    public DbSet<Kierowca> Kierowcy { get; set; }
    public DbSet<Tankowanie> Tankowania { get; set; }
    public DbSet<Ubezpieczenie> Ubezpieczenia { get; set; }
    
    public DbSet<WpisSerwisowy> WpisSerwisowy { get; set; }
    public DbSet<HarmonogramPrzegladow> Harmonogramy { get; set; }
    public DbSet<Przydzial> Przydzialy { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfiguracja indeksów (to już miałeś)
        modelBuilder.Entity<Pojazd>()
            .HasIndex(p => p.NumerRejestracyjny)
            .IsUnique();

        modelBuilder.Entity<Kierowca>()
            .HasIndex(k => k.NumerPrawaJazdy)
            .IsUnique();

        // --- FIX NA BŁĄD TANKOWANIA ---
        // Wymuszamy, żeby ID było traktowane jako Identity (auto-numerowanie)
        modelBuilder.Entity<Tankowanie>()
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();
        // -----------------------------
    }
}