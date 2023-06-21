using Sales.Shared.DTOs;
using Sales.API.Data.Entities;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllAsync(PaginationDto pagination);
        Task<IEnumerable<Category>> GetAllDeletedAsync(PaginationDto pagination);
        Task<Category> GetByIdActiveAsync(int id);
        Task<Category> GetCategoryIfExist(string name);
        Task<double> GetPages(PaginationDto pagination, bool deleted);
    }  
}
