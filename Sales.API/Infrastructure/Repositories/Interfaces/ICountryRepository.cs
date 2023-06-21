using Sales.Shared.DTOs;
using Sales.API.Data.Entities;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<double> GetPages(PaginationDto pagination);
        Task<IEnumerable<Country>> GetAllAsync(PaginationDto pagination);
        Task<Country> GetCountryIfExist(string name);
    }
}
