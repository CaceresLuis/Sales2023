using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.DTOs
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        [Display(Name = "Imagen")]
        public string Image { get; set; } = string.Empty;
    }
}
