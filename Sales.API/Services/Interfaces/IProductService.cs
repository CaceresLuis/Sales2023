using Sales.Shared.DTOs;

namespace Sales.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<bool> AddProductAsync(SimpleProductDto productDto);
    }
}
