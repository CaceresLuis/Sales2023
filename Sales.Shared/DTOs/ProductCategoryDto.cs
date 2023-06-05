namespace Sales.Shared.DTOs
{
    public class ProductCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
}