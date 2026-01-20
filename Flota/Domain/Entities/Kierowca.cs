using System.ComponentModel.DataAnnotations;

namespace Flota.Domain.Entities;

public class Kierowca
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Imię jest wymagane")]
    public string Imie { get; set; } = "";

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    public string Nazwisko { get; set; } = "";

    [Required(ErrorMessage = "Nr Prawa Jazdy jest wymagany")]
    public string NumerPrawaJazdy { get; set; } = "";

    public string Email { get; set; } = "";
    public string Telefon { get; set; } = "";
    public string Adres { get; set; } = "";
    public string Pesel { get; set; } = "";
    public int LataDoswiadczenia { get; set; }

    public string PelneImie => $"{Imie} {Nazwisko}";
}