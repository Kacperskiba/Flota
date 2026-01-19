using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flota.Domain.Attributes;

namespace Flota.Domain.Entities;

public class Ubezpieczenie
{
    [Key]
    public int Id { get; set; }

    public int PojazdId { get; set; }
    
    [ForeignKey("PojazdId")]
    public virtual Pojazd Pojazd { get; set; }

    [Required, MaxLength(50)]
    [FlotaDisplay("Nr Polisy", 1)]
    public string NumerPolisy { get; set; } = "";

    [Required, MaxLength(100)]
    [FlotaDisplay("Ubezpieczyciel", 2)]
    public string Ubezpieczyciel { get; set; } = "";

    [FlotaDisplay("Ważne od", 3)]
    public DateTime DataRozpoczecia { get; set; }

    [FlotaDisplay("Ważne do", 4)]
    public DateTime DataZakonczenia { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [FlotaDisplay("Koszt", 5)]
    public decimal Koszt { get; set; }
}