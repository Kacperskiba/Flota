using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flota.Domain.Entities;

public class Tankowanie
{
    [Key] 
    public int Id { get; set; }

    public int PojazdId { get; set; }
    public virtual Pojazd Pojazd { get; set; } = null!;

    public DateTime Data { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(10,2)")] 
    public decimal IloscLitrow { get; set; }

    [Column(TypeName = "decimal(10,2)")] 
    public decimal CenaZaLitr { get; set; }
    
    public bool CzyDoPelna { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal LacznyKoszt { get; set; }
}