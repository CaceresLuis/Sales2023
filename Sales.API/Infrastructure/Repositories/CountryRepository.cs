using Sales.API.Data;
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

        public async Task<bool> CountryExisteAsync(string name)
        {
            return await _context.Countries.AnyAsync(c => c.Name == name);
        }

        public new async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await _context.Countries
                .Include(c => c.States)
                .ThenInclude(s => s.Cities)
                .ToListAsync();
        }

        public async Task<IEnumerable<Country>> GetAllCountriesWhitEstatesAsync()
        {
            return await _context.Countries
                .Include(c => c.States)
                .ToListAsync();
        }

        public new async Task<Country> GetByIdAsync(int id)
        {
            return await _context.Countries
                .Include(c => c.States)
                .ThenInclude(s => s.Cities)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
