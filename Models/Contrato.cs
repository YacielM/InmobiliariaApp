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

        // Terminación anticipada
        public DateTime? FechaTerminacionAnticipada { get; set; }
        public decimal? MultaTerminacion { get; set; }

        // Auditoría
        public string? CreadoPor { get; set; }
        public string? TerminadoPor { get; set; }

        // marcar si la multa ya fue pagada
        public bool MultaPagada { get; set; } = false;

        [ValidateNever]
        public Inmueble Inmueble { get; set; }

        [ValidateNever]
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

        public string Estado
        {
            get
            {
                // 1. Multado (solo si la multa no fue pagada)
                if (FechaTerminacionAnticipada.HasValue && MultaTerminacion.HasValue && MultaTerminacion.Value > 0 && !MultaPagada)
                    return "Multado";

                // 2. Finalizado por multa pagada
                if (FechaTerminacionAnticipada.HasValue && MultaTerminacion.HasValue && MultaTerminacion.Value > 0 && MultaPagada)
                    return "Finalizado";

                // 3. Vigente
                if (FechaInicio <= DateTime.Today && FechaFin >= DateTime.Today)
                    return "Vigente";

                // 4. Finalizado
                if (FechaFin < DateTime.Today)
                    return "Finalizado";

                // 5. Pendiente
                if (FechaInicio > DateTime.Today)
                    return "Pendiente";

                return "Desconocido";
            }
        }

    }
}
