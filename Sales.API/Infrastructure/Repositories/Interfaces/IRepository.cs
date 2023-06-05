namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<T>> GetAllAsync();
    }
}
