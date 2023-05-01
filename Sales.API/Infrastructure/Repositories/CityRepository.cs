using Sales.API.Data;
using Sales.API.Data.Entities;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Infrastructure.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly SalesDataContex _context;
        public CityRepository(SalesDataContex context) : base(context)
        {
            _context = context;
        }
    }
}
