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

        public async Task<bool> GetCountryByName(string name)
        {
            return await _context.Countries.AnyAsync(c => c.Name == name);
        }
    }
}
