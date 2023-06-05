using Sales.Shared.DTOs;
using Sales.API.Data.Entities;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(PaginationDto pagination);
        Task<Product> GetByIdAsync(int id);
        Task<double> GetPages(PaginationDto pagination);
    }
}
