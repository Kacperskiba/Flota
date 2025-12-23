using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flota.Domain.Entities;
namespace Flota.Domain.Interfaces;

public interface IPojazdSerwis {
    Task<List<Pojazd>> PobierzWszystkieAsync();
    Task DodajAsync(Pojazd p);
}
public interface IKierowcaSerwis {
    Task<List<Kierowca>> PobierzWszystkichAsync();
    Task DodajAsync(Kierowca k);
}
public interface ITankowanieSerwis {
    Task DodajAsync(Tankowanie t);
    event Action<string> OnTankowanieDodane;
}