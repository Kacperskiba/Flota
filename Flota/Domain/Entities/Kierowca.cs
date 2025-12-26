using System.ComponentModel.DataAnnotations;

namespace Flota.Domain.Entities;

public class Kierowca
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Imie { get; set; } = "";

    [Required, MaxLength(50)]
    public string Nazwisko { get; set; } = "";

    [Required, MaxLength(20)]
    public string NumerPrawaJazdy { get; set; } = "";

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(11)]
    public string? NrTelefonu { get; set; } // Zmieniona nazwa zgodnie z tabelą

    [MaxLength(100)]
    public string? Adres { get; set; } // NOWE POLE

    public int LataDoswiadczenia { get; set; } // NOWE POLE
    
    // Imię i nazwisko jako jeden string (wygodne do wyświetlania)
    public string PelneImie => $"{Imie} {Nazwisko}";
}