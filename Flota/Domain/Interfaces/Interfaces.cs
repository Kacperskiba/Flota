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
}

public interface ITankowanieSerwis {
    Task<List<Tankowanie>> PobierzWszystkieAsync();
    Task DodajAsync(Tankowanie t);
}

public interface ISerwisPojazduSerwis {
    Task<List<ZgloszenieSerwisowe>> PobierzWszystkieAsync();
    Task DodajAsync(ZgloszenieSerwisowe z);
}
public interface IPrzydzialSerwis {
    Task<List<Przydzial>> PobierzAktywneAsync();
    Task<List<Przydzial>> PobierzHistorieAsync();
    Task WydajPojazdAsync(Przydzial p);
    Task ZwrocPojazdAsync(int przydzialId, decimal przebiegKoncowy, DateTime dataZwrotu);
}