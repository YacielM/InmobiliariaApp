using System.ComponentModel.DataAnnotations;

namespace InmobiliariaApp.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; }

        [Required, StringLength(50)]
        public string Apellido { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Clave { get; set; }

        [Required]
        public string Rol { get; set; } // "Administrador" o "Empleado"

        public string? Avatar { get; set; }
    }
}
