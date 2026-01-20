using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flota.Domain.Entities;

public class Tankowanie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // <--- TO NAPRAWIA TWÓJ BŁĄD
    public int Id { get; set; }

    public int PojazdId { get; set; }
    public virtual Pojazd? Pojazd { get; set; }

    public int KierowcaId { get; set; }
    public virtual Kierowca? Kierowca { get; set; }

    public DateTime Data { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal IloscLitrow { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CenaZaLitr { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal LacznyKoszt { get; set; }

    public bool CzyDoPelna { get; set; }
}