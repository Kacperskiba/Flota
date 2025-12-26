using System.ComponentModel.DataAnnotations;

namespace Flota.Domain.Entities;

public class Kierowca
{
    [Key] 
    public int Id { get; set; }

    [Required(ErrorMessage = "Imię jest wymagane"), MaxLength(50)] 
    public string Imie { get; set; } = "";

    [Required(ErrorMessage = "Nazwisko jest wymagane"), MaxLength(50)] 
    public string Nazwisko { get; set; } = "";

    [Required(ErrorMessage = "Numer prawa jazdy jest wymagany"), MaxLength(20)] 
    public string NumerPrawaJazdy { get; set; } = "";
    
    
    public string? Telefon { get; set; }
    public string? Email { get; set; }
    public DateTime? DataZatrudnienia { get; set; } = DateTime.Now;
}