using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InmobiliariaApp.Models
{
    public class Contrato
    {
        [Key]
        public int IdContrato { get; set; }

        [Required]
        public int IdInmueble { get; set; }

        [Required]
        public int IdInquilino { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        public decimal MontoMensual { get; set; }

        [ValidateNever] // evita que el binder lo marque como requerido
        public Inmueble Inmueble { get; set; }

        [ValidateNever] // idem para inquilino
        public Inquilino Inquilino { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaFin < FechaInicio)
        {
            yield return new ValidationResult(
                "La fecha de fin no puede ser anterior a la fecha de inicio.",
                new[] { nameof(FechaFin) });
        }
    }
    }
}
