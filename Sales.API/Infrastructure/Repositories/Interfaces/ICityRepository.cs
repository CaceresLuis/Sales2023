using Sales.API.Data.Entities;
using Sales.API.Infrastructure.Exceptions;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface ICityRepository : IRepository<City>
    {
        Task<ErrorClass> ExistCityInStateAsync(int stateId, string cityName);
    }
}
