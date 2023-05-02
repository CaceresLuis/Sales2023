using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.DTOs
{
    public class CountryDto
    {
        public int Id { get; set; }

        [Display(Name = "Pais")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El campos {0} no puede tener mas de {1} caracteres")]
        public string Name { get; set; } = string.Empty;

        public ICollection<StateDto>? States { get; set; }
        public int StatesNumber => States == null ? 0 : States.Count;

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, States: {States}";
        }
    }
}
