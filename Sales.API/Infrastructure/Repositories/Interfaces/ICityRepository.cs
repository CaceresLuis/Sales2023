using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Sales.API.Infrastructure.Exceptions;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface ICityRepository : IRepository<City>
    {
        Task<double> GetPages(PaginationDto pagination);
        Task<IEnumerable<City>> GetAllAsync(int stateId);
        Task<IEnumerable<City>> GetAllAsync(PaginationDto pagination);
        Task<ErrorClass> ExistCityInStateAsync(int stateId, string cityName);
    }
}
