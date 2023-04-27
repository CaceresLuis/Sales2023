using Sales.API.Data;
using Sales.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly SalesDataContex _context;
        public CategoryRepository(SalesDataContex context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CategoryExisteAsysn(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name == name);
        }
    }
}
