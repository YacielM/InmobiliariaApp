using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InmobiliariaApp.Models
{
    public class Inmueble
    {
        [Key]
        public int IdInmueble { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Uso { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        public int Ambientes { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public int IdPropietario { get; set; }

        [ValidateNever] //evita que el binder lo marque como requerido
        public Propietario Propietario { get; set; }
    }
}
