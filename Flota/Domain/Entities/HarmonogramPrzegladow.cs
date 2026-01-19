using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flota.Domain.Entities;

public class HarmonogramPrzegladow
{
    [Key]
    public int Id { get; set; }

    public int PojazdId { get; set; }
    // Nawigacja do pojazdu
    public virtual Pojazd Pojazd { get; set; } = null!;

    // Co ile km robić przegląd (np. 15000)
    public int InterwalKm { get; set; }

    // Co ile dni robić przegląd (np. 365)
    public int InterwalDni { get; set; }

    // Kiedy był ostatni przegląd
    public DateTime DataOstatniegoPrzegladu { get; set; }

    // Przy jakim przebiegu był ostatni przegląd
    [Column(TypeName = "decimal(12,1)")]
    public decimal PrzebiegOstatniegoPrzegladu { get; set; }
    
    // Metoda pomocnicza (Logika biznesowa ze strony 9 specyfikacji)
    public bool CzyWymaganyPrzeglad(decimal aktualnyPrzebieg)
    {
        var dniMinely = (DateTime.Now - DataOstatniegoPrzegladu).TotalDays >= InterwalDni;
        var kmMinely = (aktualnyPrzebieg - PrzebiegOstatniegoPrzegladu) >= InterwalKm;

        return dniMinely || kmMinely;
    }
}