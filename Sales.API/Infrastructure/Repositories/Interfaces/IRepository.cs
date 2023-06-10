namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> GetByIdAsync(int id);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<T>> GetAllAsync();
    }
}
