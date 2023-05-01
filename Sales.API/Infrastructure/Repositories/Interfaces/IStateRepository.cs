using Sales.API.Data.Entities;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface IStateRepository : IRepository<State>
    {
        Task<State> GetByIdWhitEstatesAsync(int id);
        Task<IEnumerable<State>> GetAllWhitEstatesAsync();
    }
}
