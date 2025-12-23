using Microsoft.EntityFrameworkCore;
using Flota.Domain.Entities;
namespace Flota.Infrastructure.Data;

public class FleetDbContext : DbContext {
    public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options) { }
    public DbSet<Pojazd> Pojazdy { get; set; }
    public DbSet<SamochodOsobowy> SamochodyOsobowe { get; set; }
    public DbSet<SamochodCiezarowy> SamochodyCiezarowe { get; set; }
    public DbSet<Kierowca> Kierowcy { get; set; }
    public DbSet<Tankowanie> Tankowania { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Pojazd>().HasDiscriminator<string>("Typ").HasValue<SamochodOsobowy>("Osobowy").HasValue<SamochodCiezarowy>("Ciezarowy");
        modelBuilder.Entity<Pojazd>().HasIndex(p => p.NumerRejestracyjny).IsUnique();
    }
}