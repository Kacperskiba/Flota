using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Flota.Domain.Entities;
using Flota.Domain.Interfaces;
using Flota.Infrastructure.Data;

namespace Flota.Infrastructure.Services;

public class PojazdSerwis : IPojazdSerwis {
    private readonly FleetDbContext _context;
    public PojazdSerwis(FleetDbContext context) { _context = context; }
    public async Task<List<Pojazd>> PobierzWszystkieAsync() => await _context.Pojazdy.ToListAsync();
    public async Task DodajAsync(Pojazd p) { _context.Pojazdy.Add(p); await _context.SaveChangesAsync(); }
    public async Task UsunAsync(int id)
    {
        var pojazd = await _context.Pojazdy.FindAsync(id);
        if (pojazd != null)
        {
            _context.Pojazdy.Remove(pojazd);
            await _context.SaveChangesAsync();
        }
    }
}

public class KierowcaSerwis : IKierowcaSerwis
{
    private readonly FleetDbContext _context;
    public KierowcaSerwis(FleetDbContext context) { _context = context; }

    public async Task<List<Kierowca>> PobierzWszystkichAsync() 
        => await _context.Kierowcy.ToListAsync();

    public async Task DodajAsync(Kierowca k) 
    {
        _context.Kierowcy.Add(k);
        await _context.SaveChangesAsync();
    }
    public async Task UsunAsync(int id)
    {
        var kierowca = await _context.Kierowcy.FindAsync(id);
        if (kierowca != null)
        {
            _context.Kierowcy.Remove(kierowca);
            await _context.SaveChangesAsync();
        }
    }
}
public class TankowanieSerwis : ITankowanieSerwis
{
    private readonly FleetDbContext _context;
    public TankowanieSerwis(FleetDbContext context) { _context = context; }

    public async Task<List<Tankowanie>> PobierzWszystkieAsync()
    {
        // Include(t => t.Pojazd) pozwala wyświetlić markę auta w tabeli tankowań
        return await _context.Tankowania
            .Include(t => t.Pojazd) 
            .OrderByDescending(t => t.Data)
            .ToListAsync();
    }

    public async Task DodajAsync(Tankowanie t)
    {
        _context.Tankowania.Add(t);
        await _context.SaveChangesAsync();
    }
}

public class SerwisPojazduSerwis : ISerwisPojazduSerwis
{
    private readonly FleetDbContext _context;
    public SerwisPojazduSerwis(FleetDbContext context) { _context = context; }

    public async Task<List<ZgloszenieSerwisowe>> PobierzWszystkieAsync()
    {
        return await _context.ZgloszeniaSerwisowe
            .Include(z => z.Pojazd)
            .OrderByDescending(z => z.DataZgloszenia)
            .ToListAsync();
    }

    public async Task DodajAsync(ZgloszenieSerwisowe z)
    {
        // Kiedy dodajemy zgłoszenie, auto automatycznie idzie "W Serwisie"
        var pojazd = await _context.Pojazdy.FindAsync(z.PojazdId);
        if (pojazd != null)
        {
            pojazd.Status = Domain.Enums.StatusPojazdu.WSerwisie;
        }

        z.Status = "W Trakcie"; // Domyślny status
        _context.ZgloszeniaSerwisowe.Add(z);
        await _context.SaveChangesAsync();
    }
    public async Task ZmienStatusAsync(int id, string nowyStatus)
    {
        var zgloszenie = await _context.ZgloszeniaSerwisowe
            .Include(z => z.Pojazd)
            .FirstOrDefaultAsync(z => z.Id == id);

        if (zgloszenie == null) return;

        zgloszenie.Status = nowyStatus;

        // LOGIKA AUTOMATYCZNEJ ZMIANY STATUSU POJAZDU
        if (zgloszenie.Pojazd != null)
        {
            if (nowyStatus == "Zakończone")
            {
                // Jeśli naprawa skończona -> auto dostępne
                zgloszenie.Pojazd.Status = Domain.Enums.StatusPojazdu.Dostepny;
            }
            else if (nowyStatus == "W Trakcie" || nowyStatus == "Planowane")
            {
                // Jeśli naprawa trwa -> auto w serwisie
                zgloszenie.Pojazd.Status = Domain.Enums.StatusPojazdu.WSerwisie;
            }
        }

        await _context.SaveChangesAsync();
    }
}
public class PrzydzialSerwis : IPrzydzialSerwis
{
    private readonly FleetDbContext _context;
    public PrzydzialSerwis(FleetDbContext context) { _context = context; }

    public async Task<List<Przydzial>> PobierzAktywneAsync()
    {
        return await _context.Przydzialy
            .Include(p => p.Pojazd).Include(p => p.Kierowca)
            .Where(p => p.DataZakonczenia == null) // Tylko te, które trwają
            .ToListAsync();
    }
    
    public async Task<List<Przydzial>> PobierzHistorieAsync()
    {
         return await _context.Przydzialy
            .Include(p => p.Pojazd).Include(p => p.Kierowca)
            .Where(p => p.DataZakonczenia != null)
            .OrderByDescending(p => p.DataZakonczenia)
            .ToListAsync();
    }

    public async Task WydajPojazdAsync(Przydzial p)
    {
        // 1. SPRAWDZENIE: Czy kierowca nie ma już aktywnego pojazdu?
        bool kierowcaZajety = await _context.Przydzialy
            .AnyAsync(x => x.KierowcaId == p.KierowcaId && x.DataZakonczenia == null);

        if (kierowcaZajety)
        {
            throw new Exception("Ten kierowca ma już przypisany aktywny pojazd! Najpierw zwróć poprzedni.");
        }

        // 2. SPRAWDZENIE: Czy pojazd na pewno jest dostępny?
        var pojazd = await _context.Pojazdy.FindAsync(p.PojazdId);
        if (pojazd == null) throw new Exception("Pojazd nie istnieje");
    
        // To blokuje wydanie auta będącego w serwisie lub u innego kierowcy
        if (pojazd.Status != Domain.Enums.StatusPojazdu.Dostepny)
        {
            throw new Exception($"Pojazd nie jest dostępny! Jego status to: {pojazd.Status}");
        }

        // Reszta logiki bez zmian...
        p.DataRozpoczecia = DateTime.Now;
        p.PrzebiegPoczatkowy = pojazd.Przebieg;
        p.DataZakonczenia = null;

        pojazd.Status = Domain.Enums.StatusPojazdu.WUzytkowaniu;

        _context.Przydzialy.Add(p);
        await _context.SaveChangesAsync();
    }

    public async Task ZwrocPojazdAsync(int przydzialId, decimal przebiegKoncowy, DateTime dataZwrotu)
    {
        var przydzial = await _context.Przydzialy.Include(p => p.Pojazd).FirstOrDefaultAsync(p => p.Id == przydzialId);
        if (przydzial == null) throw new Exception("Nie znaleziono przydziału");

        // 1. Zamknij przydział
        przydzial.DataZakonczenia = dataZwrotu;
        przydzial.PrzebiegKoncowy = przebiegKoncowy;

        // 2. Zaktualizuj przebieg pojazdu w bazie głównej
        if (przydzial.Pojazd != null)
        {
            przydzial.Pojazd.Przebieg = przebiegKoncowy;
            przydzial.Pojazd.Status = Domain.Enums.StatusPojazdu.Dostepny; // Pojazd wraca do puli
        }

        await _context.SaveChangesAsync();
    }
}