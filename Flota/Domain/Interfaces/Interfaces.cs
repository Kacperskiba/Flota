using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flota.Domain.Entities;
namespace Flota.Domain.Interfaces;

public interface IPojazdSerwis {
    Task<List<Pojazd>> PobierzWszystkieAsync();
    Task DodajAsync(Pojazd p);
    Task UsunAsync(int id);
}
public interface IKierowcaSerwis {
    Task<List<Kierowca>> PobierzWszystkichAsync();
    Task DodajAsync(Kierowca k);
    Task UsunAsync(int id);
}

public interface ITankowanieSerwis {
    Task<List<Tankowanie>> PobierzWszystkieAsync();
    Task DodajAsync(Tankowanie t);
}

public interface ISerwisPojazdu {
    Task<List<WpisSerwisowy>> PobierzWszystkieAsync();
    Task DodajAsync(WpisSerwisowy z);
    Task ZmienStatusAsync(int id, string nowyStatus);
}
public interface IPrzydzialSerwis {
    Task<List<Przydzial>> PobierzAktywneAsync();
    Task<List<Przydzial>> PobierzHistorieAsync();
    Task WydajPojazdAsync(Przydzial p);
    Task ZwrocPojazdAsync(int przydzialId, decimal przebiegKoncowy, DateTime dataZwrotu);
}
public interface IUbezpieczenieSerwis {
    Task<List<Ubezpieczenie>> PobierzDlaPojazduAsync(int pojazdId);
    Task DodajAsync(Ubezpieczenie u);
    Task UsunAsync(int id);
    Task<List<Ubezpieczenie>> PobierzWygasajaceAsync(int dni);
}
public interface IHarmonogramSerwis
{
    Task<HarmonogramPrzegladow?> PobierzDlaPojazduAsync(int pojazdId);
    Task UstawHarmonogramAsync(HarmonogramPrzegladow h);
}