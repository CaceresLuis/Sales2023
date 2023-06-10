using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Sales.API.Infrastructure.Exceptions;

namespace Sales.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<CustomResponse> AddProductAsync(SimpleProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync(PaginationDto pagination);
        Task<IEnumerable<Product>> GetAllDeltedAsync(PaginationDto pagination);
        Task<Product> GetByIdActiveAsync(int id);
        Task<Product> GetDeleteByIdAsync(int id);
        Task<double> GetPages(PaginationDto pagination);
        Task<CustomResponse> UpdateProductAsync(SimpleProductDto productDto);
    }
}
