using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly SalesDataContex _context;
        public ProductRepository(SalesDataContex context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> NameProductExistAsync(string name) => await _context.Products.AnyAsync(p => p.Name.ToLower() == name.ToLower());

        public async Task<Product> GetByIdActiveAsync(int id) => await _context.Products.Include(p => p.ProductImages)
            .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        public async Task<Product> GetDeleteByIdAsync(int id) => await _context.Products.Include(p => p.ProductImages)
            .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted);

        public async Task<IEnumerable<Product>> GetAllAsync(PaginationDto pagination)
        {
            IQueryable<Product> queriable = _context.Products.Include(p => p.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).Where(p => !p.IsDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
                queriable = queriable.Where(c => c.Name.ToLower().Contains(pagination.Filter.ToLower()));

            return await queriable.OrderBy(p => p.Name).Paginate(pagination).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllDeltedAsync(PaginationDto pagination)
        {
            IQueryable<Product> queriable = _context.Products.Include(p => p.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).Where(p => p.IsDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
                queriable = queriable.Where(c => c.Name.ToLower().Contains(pagination.Filter.ToLower()));

            return await queriable.OrderBy(p => p.Name).Paginate(pagination).ToListAsync();
        }

        public async Task<double> GetPages(PaginationDto pagination)
        {
            IQueryable<Product> queriable = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
                queriable = queriable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));

            double count = await queriable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return totalPages;
        }
    }
}
