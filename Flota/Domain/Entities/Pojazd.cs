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
    public int RokProdukcji { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PojemnoscZbiornika { get; set; }

    [Required, MaxLength(20)]
    [FlotaDisplay("Nr Rej.", 4)]
    public string NumerRejestracyjny { get; set; } = "";

    [Column(TypeName = "decimal(12,1)")]
    [FlotaDisplay("Przebieg", 5)]
    public decimal Przebieg { get; set; }

    [FlotaDisplay("Status", 6)]
    public StatusPojazdu Status { get; set; }

    public virtual ICollection<Tankowanie> Tankowania { get; set; } = new List<Tankowanie>();

    public virtual ICollection<Przydzial> Przydzialy { get; set; } = new List<Przydzial>();

    public virtual ICollection<WpisSerwisowy> ZgloszeniaSerwisowe { get; set; } = new List<WpisSerwisowy>();

    public virtual ICollection<Ubezpieczenie> Ubezpieczenie { get; set; } = new List<Ubezpieczenie>();
    
    // Pola opcjonalne (zostawiamy je, bo mogą się przydać, ale nie są wymagane w Twoim nowym schemacie)
    public int? LiczbaMiejsc { get; set; }
}