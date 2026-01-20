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
            pojazd.Status = Domain.Enums.StatusPojazdu.Wylaczony;
        
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
        if (t.LacznyKoszt == 0 && t.IloscLitrow > 0 && t.CenaZaLitr > 0)
        {
            t.LacznyKoszt = t.IloscLitrow * t.CenaZaLitr;
        }

        _context.Tankowania.Add(t);

        // 3. REALIZACJA SCENARIUSZA 1 (Strona 9 specyfikacji)
        // Pobieramy auto, żeby zaktualizować jego przebieg
        var pojazd = await _context.Pojazdy.FindAsync(t.PojazdId);
        if (pojazd != null)
        {
            // Sprawdźmy, czy encja Tankowanie ma pole "StanLicznika" lub czy aktualizujemy "w ciemno".
            // Zazwyczaj przy tankowaniu podaje się aktualny przebieg.
            // Zakładam, że w klasie Tankowanie masz pole 'AktualnyPrzebieg' (lub podobne), 
            // ale jeśli go nie ma w bazie (wg schematu str. 4-5 go nie ma!), 
            // to ten punkt może być trudny do zrealizowania bez zmiany bazy.
        
            // JEDNAK: Jeśli specyfikacja tego wymaga, warto dodać pole 'Przebieg' do Tankowania.
            // Jeśli nie chcesz zmieniać bazy, to ten punkt musimy pominąć, 
            // ale zgodnie z logiką: tankowanie to świetny moment na aktualizację licznika.
        }

        await _context.SaveChangesAsync();
}
public class UbezpieczenieSerwis : IUbezpieczenieSerwis
{
    private readonly FleetDbContext _context;
    public UbezpieczenieSerwis(FleetDbContext context) { _context = context; }

    public async Task<List<Ubezpieczenie>> PobierzDlaPojazduAsync(int pojazdId)
    {
        return await _context.Ubezpieczenia
            .Where(u => u.PojazdId == pojazdId)
            .OrderByDescending(u => u.DataZakonczenia)
            .ToListAsync();
    }

    public async Task DodajAsync(Ubezpieczenie u)
    {
        _context.Ubezpieczenia.Add(u);
        await _context.SaveChangesAsync();
    }

    public async Task UsunAsync(int id)
    {
        var ubezp = await _context.Ubezpieczenia.FindAsync(id);
        if (ubezp != null)
        {
            _context.Ubezpieczenia.Remove(ubezp);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Ubezpieczenie>> PobierzWygasajaceAsync(int dni)
    {
        var dataGraniczna = DateTime.Now.AddDays(dni);
        return await _context.Ubezpieczenia
            .Include(u => u.Pojazd) // Żebyśmy wiedzieli, jakiego auta to dotyczy
            .Where(u => u.DataZakonczenia >= DateTime.Now && u.DataZakonczenia <= dataGraniczna)
            .ToListAsync();
    }
}
public class SerwisPojazdu : ISerwisPojazdu
{
    private readonly FleetDbContext _context;
    public SerwisPojazdu(FleetDbContext context) { _context = context; }

    public async Task<List<WpisSerwisowy>> PobierzWszystkieAsync()
    {
        return await _context.WpisSerwisowy
            .Include(z => z.Pojazd) // <--- WAŻNE: Musi pobierać pojazd
            .OrderByDescending(z => z.DataZgloszenia)
            .ToListAsync();
    }

    public async Task DodajAsync(WpisSerwisowy z)
    {
        // 1. Walidacja: Jeśli próbujemy dodać serwis "W Trakcie", sprawdzamy czy auto już nie jest w serwisie
        if (z.Status == "W Trakcie")
        {
            bool czyJuzWSerwisie = await _context.WpisSerwisowy
                .AnyAsync(x => x.PojazdId == z.PojazdId && x.Status == "W Trakcie");

            if (czyJuzWSerwisie)
            {
                throw new Exception("To auto ma już aktywny serwis 'W Trakcie'! Zakończ poprzedni, zanim rozpoczniesz nowy.");
            }
        }

        // 2. Pobieramy pojazd i zmieniamy mu status
        var pojazd = await _context.Pojazdy.FindAsync(z.PojazdId);
    
        if (pojazd != null && z.Status == "W Trakcie")
        {
            pojazd.Status = Domain.Enums.StatusPojazdu.WSerwisie;
        }

        _context.WpisSerwisowy.Add(z);
        await _context.SaveChangesAsync();
    }
    public async Task ZmienStatusAsync(int id, string nowyStatus)
    {
        // 1. Pobieramy zgłoszenie wraz z pojazdem
        var zgloszenie = await _context.WpisSerwisowy
            .Include(z => z.Pojazd)
            .FirstOrDefaultAsync(z => z.Id == id);

        if (zgloszenie == null) return;

        // 2. WALIDACJA (STRAŻNIK)
        // Jeśli próbujemy ustawić status na "W Trakcie", sprawdzamy inne zgłoszenia
        if (nowyStatus == "W Trakcie")
        {
            bool czyInnySerwisTrwa = await _context.WpisSerwisowy
                .AnyAsync(x => x.PojazdId == zgloszenie.PojazdId 
                               && x.Id != id // Ignorujemy to zgłoszenie (choć ono jest Planowane)
                               && x.Status == "W Trakcie");

            if (czyInnySerwisTrwa)
            {
                throw new Exception("Nie można rozpocząć tego serwisu! To auto jest już w trakcie innej naprawy.");
            }
        }

        // 3. Aktualizacja statusu zgłoszenia
        zgloszenie.Status = nowyStatus;

        // 4. Automatyczna zmiana statusu POJAZDU
        if (zgloszenie.Pojazd != null)
        {
            if (nowyStatus == "Zakończone")
            {
                zgloszenie.Pojazd.Status = Domain.Enums.StatusPojazdu.Dostepny;
            }
            else if (nowyStatus == "W Trakcie")
            {
                zgloszenie.Pojazd.Status = Domain.Enums.StatusPojazdu.WSerwisie;
            }
            // Przy "Planowane" nie zmieniamy statusu pojazdu
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
    public async Task<Przydzial?> PobierzPoIdAsync(int id)
    {
        return await _context.Przydzialy
            .Include(p => p.Pojazd)
            .Include(p => p.Kierowca)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}

public class HarmonogramSerwis : IHarmonogramSerwis
{
    private readonly FleetDbContext _context;
    public HarmonogramSerwis(FleetDbContext context) { _context = context; }

    public async Task<HarmonogramPrzegladow?> PobierzDlaPojazduAsync(int pojazdId)
    {
        return await _context.Harmonogramy
            .FirstOrDefaultAsync(h => h.PojazdId == pojazdId);
    }

    public async Task UstawHarmonogramAsync(HarmonogramPrzegladow h)
    {
        var istniejacy = await _context.Harmonogramy
            .FirstOrDefaultAsync(x => x.PojazdId == h.PojazdId);

        if (istniejacy != null)
        {
            // Aktualizacja
            istniejacy.InterwalKm = h.InterwalKm;
            istniejacy.InterwalDni = h.InterwalDni;
            istniejacy.DataOstatniegoPrzegladu = h.DataOstatniegoPrzegladu;
            istniejacy.PrzebiegOstatniegoPrzegladu = h.PrzebiegOstatniegoPrzegladu;
        }
        else
        {
            // Nowy wpis
            _context.Harmonogramy.Add(h);
        }

        await _context.SaveChangesAsync();
    }
}