using Sales.API.Data.Entities;
using Sales.API.Infrastructure.Exceptions;
using Sales.Shared.DTOs;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface ICityRepository : IRepository<City>
    {
        Task<ErrorClass> ExistCityInStateAsync(int stateId, string cityName);
        Task<IEnumerable<City>> GetAllAsync(PaginationDto pagination);
        Task<IEnumerable<City>> GetAllAsync(int stateId);
        Task<double> GetPages(PaginationDto pagination);
    }
}
