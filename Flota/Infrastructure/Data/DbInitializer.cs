using System;
using System.Collections.Generic;
using System.Linq;
using Flota.Domain.Entities;
using Flota.Domain.Enums;

namespace Flota.Infrastructure.Data;

public static class DbInitializer 
{
    public static void Initialize(FleetDbContext context) 
    {
        // 1. HARD RESET BAZY (Wyczyść stare, wgraj nowe - OGROMNE)
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // ==========================================
        // 1. GENEROWANIE KIEROWCÓW (30 Osób)
        // ==========================================
        var imiona = new[] { "Adam", "Bartek", "Cezary", "Dariusz", "Edward", "Filip", "Grzegorz", "Henryk", "Igor", "Jan", "Kamil", "Łukasz", "Marek", "Norbert", "Olaf", "Piotr", "Robert", "Sebastian", "Tomasz", "Wojciech" };
        var nazwiska = new[] { "Kowalski", "Nowak", "Wiśniewski", "Wójcik", "Kowalczyk", "Kamiński", "Lewandowski", "Zieliński", "Szymański", "Woźniak", "Dąbrowski", "Kozłowski", "Jankowski", "Mazur", "Kwiatkowski", "Krawczyk", "Kaczmarek", "Piotrowski", "Grabowski", "Zając" };
        var miasta = new[] { "Warszawa", "Kraków", "Gdańsk", "Wrocław", "Poznań", "Łódź", "Szczecin", "Bydgoszcz", "Lublin", "Katowice" };

        var kierowcy = new List<Kierowca>();
        var rand = new Random();

        for (int i = 0; i < 30; i++)
        {
            var imie = imiona[rand.Next(imiona.Length)];
            var nazwisko = nazwiska[rand.Next(nazwiska.Length)];
            kierowcy.Add(new Kierowca
            {
                Imie = imie,
                Nazwisko = nazwisko,
                NumerPrawaJazdy = $"PJ/{1000 + i}/{rand.Next(2010, 2024)}",
                Email = $"{imie.ToLower()}.{nazwisko.ToLower()}{i}@flota.pl",
                Telefon = $"50{i}-000-{rand.Next(100, 999)}",
                Adres = $"{miasta[rand.Next(miasta.Length)]}, ul. Długa {rand.Next(1, 100)}",
                LataDoswiadczenia = rand.Next(1, 30)
            });
        }
        context.Kierowcy.AddRange(kierowcy);
        context.SaveChanges();

        // ==========================================
        // 2. GENEROWANIE POJAZDÓW (40 Aut)
        // ==========================================
        var pojazdy = new List<Pojazd>();

        // FLOTA OSOBOWA (Handlowcy) - Toyoty i Skody
        for (int i = 1; i <= 15; i++)
        {
            pojazdy.Add(new Pojazd { Marka = "Toyota", Model = "Corolla", NumerRejestracyjny = $"WA 100{i:00}", RokProdukcji = rand.Next(2020, 2024), Przebieg = rand.Next(10000, 150000), PojemnoscZbiornika = 50, Status = (StatusPojazdu)rand.Next(0, 3), LiczbaMiejsc = 5 });
            pojazdy.Add(new Pojazd { Marka = "Skoda", Model = "Octavia", NumerRejestracyjny = $"KR 200{i:00}", RokProdukcji = rand.Next(2019, 2023), Przebieg = rand.Next(40000, 200000), PojemnoscZbiornika = 55, Status = (StatusPojazdu)rand.Next(0, 3), LiczbaMiejsc = 5 });
        }

        // FLOTA DOSTAWCZA - Fordy i Renault
        for (int i = 1; i <= 5; i++)
        {
            pojazdy.Add(new Pojazd { Marka = "Ford", Model = "Transit", NumerRejestracyjny = $"GD 300{i:00}", RokProdukcji = rand.Next(2017, 2022), Przebieg = rand.Next(100000, 300000), PojemnoscZbiornika = 80, Status = StatusPojazdu.WUzytkowaniu, LiczbaMiejsc = 3 });
            pojazdy.Add(new Pojazd { Marka = "Renault", Model = "Master", NumerRejestracyjny = $"PO 400{i:00}", RokProdukcji = rand.Next(2018, 2023), Przebieg = rand.Next(80000, 250000), PojemnoscZbiornika = 100, Status = (StatusPojazdu)rand.Next(0, 3), LiczbaMiejsc = 3 });
        }

        // FLOTA VIP - Mercedes i BMW
        pojazdy.Add(new Pojazd { Marka = "Mercedes", Model = "E-Klasa", NumerRejestracyjny = "W0 PREZES", RokProdukcji = 2024, Przebieg = 5000, PojemnoscZbiornika = 66, Status = StatusPojazdu.Dostepny, LiczbaMiejsc = 5 });
        pojazdy.Add(new Pojazd { Marka = "BMW", Model = "X5", NumerRejestracyjny = "W0 SZEF", RokProdukcji = 2023, Przebieg = 25000, PojemnoscZbiornika = 80, Status = StatusPojazdu.WUzytkowaniu, LiczbaMiejsc = 5 });

        context.Pojazdy.AddRange(pojazdy);
        context.SaveChanges();

        // ==========================================
        // 3. GENEROWANIE TANKOWAŃ (Masowo!)
        // ==========================================
        var tankowania = new List<Tankowanie>();
        
        foreach (var p in pojazdy)
        {
            // Pomijamy auta nowe z salonu (mały przebieg)
            if (p.Przebieg < 1000) continue;

            // Generujemy od 10 do 25 tankowań na auto (żeby wykresy były ładne)
            int iloscTankowan = rand.Next(10, 25);
            
            for (int k = 0; k < iloscTankowan; k++)
            {
                var data = DateTime.Now.AddDays(-rand.Next(1, 365)); // Losowa data z ostatniego roku
                var litry = rand.Next(30, (int)p.PojemnoscZbiornika);
                var cena = 6.00m + ((decimal)rand.NextDouble() * 1.50m); // Cena 6.00 - 7.50 zł

                tankowania.Add(new Tankowanie
                {
                    PojazdId = p.Id,
                    KierowcaId = kierowcy[rand.Next(kierowcy.Count)].Id, // Losowy kierowca
                    Data = data,
                    IloscLitrow = litry,
                    CenaZaLitr = Math.Round(cena, 2),
                    LacznyKoszt = Math.Round(litry * cena, 2),
                    CzyDoPelna = true
                });
            }
        }
        context.Tankowania.AddRange(tankowania);

        // ==========================================
        // 4. HARMONOGRAMY (Dla każdego auta)
        // ==========================================
        var harmonogramy = new List<HarmonogramPrzegladow>();
        foreach (var p in pojazdy)
        {
            bool czyStary = p.RokProdukcji < 2020;
            // Starsze auta mają przegląd wymagany "na wczoraj" (żeby świeciło na czerwono)
            var dataOstatniego = czyStary ? DateTime.Now.AddMonths(-14) : DateTime.Now.AddMonths(-rand.Next(1, 10));
            var przebiegOstatniego = p.Przebieg - rand.Next(1000, 10000);

            harmonogramy.Add(new HarmonogramPrzegladow
            {
                PojazdId = p.Id,
                InterwalKm = 15000,
                InterwalDni = 365,
                DataOstatniegoPrzegladu = dataOstatniego,
                PrzebiegOstatniegoPrzegladu = przebiegOstatniego
            });
        }
        context.Harmonogramy.AddRange(harmonogramy);

        // ==========================================
        // 5. HISTORIA SERWISOWA (Naprawy)
        // ==========================================
        var serwisy = new List<WpisSerwisowy>();
        var warsztaty = new[] { "ASO Toyota", "Serwis Skoda", "Mechanika Pana Mietka", "Bosch Service", "InterCars Warsztat" };
        var usterki = new[] { "Wymiana oleju", "Klocki hamulcowe", "Wymiana opon", "Awaria klimatyzacji", "Stukające zawieszenie", "Przegląd okresowy", "Wymiana żarówki", "Naprawa blacharska" };

        foreach (var p in pojazdy)
        {
            // Każde auto ma 1-4 wpisy w historii
            int ileWpisow = rand.Next(1, 4);
            for (int i = 0; i < ileWpisow; i++)
            {
                var typ = (TypNaprawy)rand.Next(0, 5); // Losowy typ
                serwisy.Add(new WpisSerwisowy
                {
                    PojazdId = p.Id,
                    DataZgloszenia = DateTime.Now.AddDays(-rand.Next(10, 500)),
                    Opis = usterki[rand.Next(usterki.Length)],
                    Koszt = rand.Next(200, 5000),
                    Status = "Zakończone",
                    NazwaWarsztatu = warsztaty[rand.Next(warsztaty.Length)],
                    TypNaprawy = typ
                });
            }

            // Jeśli auto jest "W Serwisie", dodajemy aktywną naprawę
            if (p.Status == StatusPojazdu.WSerwisie)
            {
                serwisy.Add(new WpisSerwisowy
                {
                    PojazdId = p.Id,
                    DataZgloszenia = DateTime.Now.AddDays(-2),
                    Opis = "DIAGNOSTYKA SILNIKA - PILNE",
                    Koszt = 0, // Jeszcze nieznany
                    Status = "W Trakcie",
                    NazwaWarsztatu = "ASO Centrala",
                    TypNaprawy = TypNaprawy.Awaria
                });
            }
        }
        context.WpisSerwisowy.AddRange(serwisy);

        // ==========================================
        // 6. UBEZPIECZENIA
        // ==========================================
        var ubezpieczyciele = new[] { "PZU", "Warta", "Allianz", "Hestia", "Link4" };
        foreach (var p in pojazdy)
        {
            context.Ubezpieczenia.Add(new Ubezpieczenie
            {
                PojazdId = p.Id,
                NumerPolisy = $"POLISA/{p.NumerRejestracyjny}/{DateTime.Now.Year}",
                Ubezpieczyciel = ubezpieczyciele[rand.Next(ubezpieczyciele.Length)],
                DataRozpoczecia = DateTime.Now.AddMonths(-rand.Next(1, 6)),
                DataZakonczenia = DateTime.Now.AddMonths(rand.Next(1, 12)),
                Koszt = rand.Next(1000, 5000)
            });
        }

        // ==========================================
        // 7. AKTYWNE PRZYDZIAŁY (Dla aut w trasie)
        // ==========================================
        foreach (var p in pojazdy)
        {
            if (p.Status == StatusPojazdu.WUzytkowaniu)
            {
                context.Przydzialy.Add(new Przydzial
                {
                    PojazdId = p.Id,
                    KierowcaId = kierowcy[rand.Next(kierowcy.Count)].Id,
                    DataRozpoczecia = DateTime.Now.AddDays(-rand.Next(1, 30)),
                    PrzebiegPoczatkowy = p.Przebieg - rand.Next(100, 2000)
                });
            }
        }

        context.SaveChanges();
    }
}