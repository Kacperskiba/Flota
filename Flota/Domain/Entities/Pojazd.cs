using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flota.Domain.Attributes;
using Flota.Domain.Enums;

namespace Flota.Domain.Entities;

public class Pojazd
{
    [Key]
    public int Id { get; set; }

    [FlotaDisplay("Marka", 1)]
    [Required, MaxLength(50)]
    public string Marka { get; set; } = "";

    [FlotaDisplay("Model", 2)]
    [Required, MaxLength(50)]
    public string Model { get; set; } = "";

    [FlotaDisplay("Rok Prod.", 3)]
    public int RokProdukcji { get; set; } // NOWE POLE

    [Column(TypeName = "decimal(10,2)")]
    public decimal PojemnoscZbiornika { get; set; }

    [Required, MaxLength(20)]
    public string NumerRejestracyjny { get; set; } = "";

    [Column(TypeName = "decimal(12,1)")]
    public decimal Przebieg { get; set; }

    public StatusPojazdu Status { get; set; }

    // Pola opcjonalne (zostawiamy je, bo mogą się przydać, ale nie są wymagane w Twoim nowym schemacie)
    public int? LiczbaMiejsc { get; set; }
}