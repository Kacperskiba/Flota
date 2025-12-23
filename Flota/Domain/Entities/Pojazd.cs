using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flota.Domain.Attributes;
using Flota.Domain.Enums;

namespace Flota.Domain.Entities;

public abstract class Pojazd
{
    [Key] 
    public int Id { get; set; }

    [FlotaDisplay("Marka", 1)] 
    [Required(ErrorMessage = "Marka jest wymagana"), MaxLength(50)] 
    public string Marka { get; set; } = "";

    [FlotaDisplay("Model", 2)] 
    [Required(ErrorMessage = "Model jest wymagany"), MaxLength(50)] 
    public string Model { get; set; } = "";

    [FlotaDisplay("Nr Rej.", 3)] 
    [Required(ErrorMessage = "Numer rejestracyjny jest wymagany"), MaxLength(20)] 
    public string NumerRejestracyjny { get; set; } = "";

    [FlotaDisplay("Przebieg", 4)] 
    [Column(TypeName = "decimal(12,1)")] 
    public decimal Przebieg { get; set; }

    public StatusPojazdu Status { get; set; }

    [Column(TypeName = "decimal(10,2)")] 
    public decimal PojemnoscZbiornika { get; set; }

    public virtual ICollection<Tankowanie> Tankowania { get; set; } = new List<Tankowanie>();
}

public class SamochodOsobowy : Pojazd
{
    public int LiczbaMiejsc { get; set; }
}

public class SamochodCiezarowy : Pojazd
{
    [Column(TypeName = "decimal(10,2)")] 
    public decimal Ladownosc { get; set; }
}