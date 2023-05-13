using Sales.Shared.DTOs;
using Sales.API.Data.Entities;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<bool> CountryExisteAsync(string name);
        Task<double> GetPages(PaginationDto pagination);
        Task<IEnumerable<Country>> GetAllAsync(PaginationDto pagination);
    }
}
