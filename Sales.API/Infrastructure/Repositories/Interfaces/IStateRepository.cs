using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Sales.API.Infrastructure.Exceptions;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface IStateRepository : IRepository<State>
    {
        Task<State> GetByIdWhitEstatesAsync(int id);
        Task<ErrorClass> ExistStateInCountry(int countryId, string nameState);
        Task<IEnumerable<State>> GetAllAsync(PaginationDto pagination);
        Task<double> GetPages(PaginationDto pagination);
        Task<IEnumerable<State>> GetAllAsync(int countryId);
    }
}
