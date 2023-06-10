using Sales.Shared.DTOs;
using Sales.API.Data.Entities;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> CategoryExisteAsysn(string name);
        Task<IEnumerable<Category>> GetAllAsync(PaginationDto pagination);
        Task<IEnumerable<Category>> GetAllDeletedAsync(PaginationDto pagination);
        Task<Category> GetByIdActiveAsync(int id);
        Task<double> GetPages(PaginationDto pagination);
    }  
}
