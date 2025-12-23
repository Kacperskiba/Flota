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
}

public class TankowanieSerwis : ITankowanieSerwis {
    private readonly FleetDbContext _context;
    public event Action<string>? OnTankowanieDodane;
    public TankowanieSerwis(FleetDbContext context) { _context = context; }
    public async Task DodajAsync(Tankowanie t) {
        _context.Tankowania.Add(t);
        await _context.SaveChangesAsync();
        OnTankowanieDodane?.Invoke($"Zatankowano {t.IloscLitrow}L");
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
}
