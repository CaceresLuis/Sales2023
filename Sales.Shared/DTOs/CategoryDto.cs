using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El campos {0} no puede tener mas de {1} caracteres")]
        public string? Name { get; set; }
        public int ProductCategoryCount { get; set; }
        public DateTime CrateAt { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeleteAt { get; set; }
        public bool Restore { get; set; }
    }
}
