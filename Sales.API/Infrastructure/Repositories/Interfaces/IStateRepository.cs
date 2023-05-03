using Sales.API.Data.Entities;
using Sales.API.Infrastructure.Exceptions;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface IStateRepository : IRepository<State>
    {
        Task<State> GetByIdWhitEstatesAsync(int id);
        Task<IEnumerable<State>> GetAllWhitEstatesAsync();
        Task<ErrorClass> ExistStateInCountry(int idCountry, string nameState);
    }
}
