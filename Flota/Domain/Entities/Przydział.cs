using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flota.Domain.Entities;

public class Przydzial
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PojazdId { get; set; }
    public virtual Pojazd Pojazd { get; set; } = null!;

    [Required]
    public int KierowcaId { get; set; }
    public virtual Kierowca Kierowca { get; set; } = null!;

    public DateTime DataRozpoczecia { get; set; } = DateTime.Now;
    public DateTime? DataZakonczenia { get; set; } // NULL jeśli aktywny

    [Column(TypeName = "decimal(12,1)")]
    public decimal PrzebiegPoczatkowy { get; set; }

    [Column(TypeName = "decimal(12,1)")]
    public decimal? PrzebiegKoncowy { get; set; } // NULL przy wydaniu
}