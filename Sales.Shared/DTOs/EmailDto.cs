using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.DTOs
{
    public class EmailDto
    {
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [EmailAddress(ErrorMessage = "Debes ingresar un correo valido")]
        public string? Email { get; set; }
    }
}
