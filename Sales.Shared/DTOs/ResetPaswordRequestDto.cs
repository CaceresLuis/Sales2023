using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.DTOs
{
    public class ResetPaswordRequestDto
    {
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [EmailAddress(ErrorMessage = "Debes ingresar un correo valido")]
        public string? Email { get; set; }
        public string UrlBase { get; set; } = string.Empty;
    }
}
