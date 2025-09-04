 using System.ComponentModel.DataAnnotations;
 namespace InmobiliariaApp.Models
 {
    public class Inquilino
    {
        [Key]
        public int IdInquilino { get; set; }
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "El DNI debe tener entre 7 y 8 caracteres.")]
        public string Dni { get; set; }
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre completo no puede exceder los 200 caracteres.")]
        public string NombreCompleto { get; set; }
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        [StringLength(150, ErrorMessage = "El email no puede exceder los 150 caracteres.")]
        public string Email { get; set; }
    }
 }