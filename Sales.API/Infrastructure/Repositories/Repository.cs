using Sales.API.Data;
using Microsoft.EntityFrameworkCore;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SalesDataContex _context;
        internal readonly DbSet<T> _dbSet;

        public Repository(SalesDataContex context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void AddAsync(T entity) => _dbSet.Add(entity);
        public void DeleteAsync(T entity) => _dbSet.Remove(entity);
        public void UpdateAsync(T entity) => _context.Update(entity);
        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
