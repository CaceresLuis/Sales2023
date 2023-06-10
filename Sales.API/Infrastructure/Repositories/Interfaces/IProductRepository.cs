using Sales.Shared.DTOs;
using Sales.API.Data.Entities;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllAsync(PaginationDto pagination);
        Task<IEnumerable<Product>> GetAllDeltedAsync(PaginationDto pagination);
        Task<Product> GetByIdActiveAsync(int id);
        Task<Product> GetDeleteByIdAsync(int id);

        //Task<Product> GetByIdAsync(int id);
        Task<double> GetPages(PaginationDto pagination);
        Task<bool> NameProductExistAsync(string name);
    }
}
