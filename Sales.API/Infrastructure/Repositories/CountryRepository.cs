using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Infrastructure.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly SalesDataContex _context;
        public CountryRepository(SalesDataContex context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Country>> GetAllAsync(PaginationDto pagination)
        {
            IQueryable<Country> queriable = _context.Countries.Include(x => x.States).AsQueryable();

            if(!string.IsNullOrWhiteSpace(pagination.Filter)) 
                queriable = queriable.Where(c => c.Name.ToLower().Contains(pagination.Filter.ToLower()));

            return await queriable.OrderBy(c => c.Name).Paginate(pagination).ToListAsync();
        }

        public new async Task<Country> GetByIdAsync(int id)
        {
            return await _context.Countries
                .Include(c => c.States)
                .ThenInclude(s => s.Cities)
            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<double> GetPages(PaginationDto pagination)
        {
            IQueryable<Country> queriable = _context.Countries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
                queriable = queriable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));

            double count = await queriable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return totalPages;
        }

        public async Task<bool> CountryExisteAsync(string name)
        {
            return await _context.Countries.AnyAsync(c => c.Name == name);
        }
    }
}
