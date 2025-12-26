using System.Linq;
using Flota.Domain.Entities;
using Flota.Domain.Enums;
namespace Flota.Infrastructure.Data;
public static class DbInitializer {
    public static void Initialize(FleetDbContext context) {
        context.Database.EnsureCreated();
        if (context.Pojazdy.Any()) return;
        
        context.Pojazdy.AddRange(
            new Pojazd { Marka="Toyota", Model="Corolla", NumerRejestracyjny="CB12345", Przebieg=10000, PojemnoscZbiornika=50, Status=StatusPojazdu.Dostepny, LiczbaMiejsc=5 }
        );
        context.SaveChanges();
    }
}