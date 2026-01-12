using Microsoft.EntityFrameworkCore;
using Flota.Domain.Entities;

namespace Flota.Infrastructure.Data;

public class FleetDbContext : DbContext 
{
    public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options) { }

    public DbSet<Pojazd> Pojazdy { get; set; }
    public DbSet<Kierowca> Kierowcy { get; set; }
    public DbSet<Tankowanie> Tankowania { get; set; }
    
    public DbSet<ZgloszenieSerwisowe> ZgloszeniaSerwisowe { get; set; }
    public DbSet<Przydzial> Przydzialy { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        modelBuilder.Entity<Pojazd>()
            .HasIndex(p => p.NumerRejestracyjny)
            .IsUnique();
            
        modelBuilder.Entity<Kierowca>()
            .HasIndex(k => k.NumerPrawaJazdy)
            .IsUnique();
    }
}