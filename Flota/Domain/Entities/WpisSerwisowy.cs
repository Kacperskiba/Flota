using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flota.Domain.Entities;

public class WpisSerwisowy
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PojazdId { get; set; }
    public virtual Pojazd Pojazd { get; set; } = null!;

    [Required(ErrorMessage = "Opis jest wymagany")]
    public string Opis { get; set; } = "";

    [Column(TypeName = "decimal(10,2)")]
    public decimal Koszt { get; set; }

    public DateTime DataZgloszenia { get; set; } = DateTime.Now;
    
    // Status: np. "W trakcie", "Zakończone"
    public string Status { get; set; } = "W Trakcie"; 
}