using Sales.API.Data;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Sales.API.Infrastructure.Exceptions;
using Sales.API.Infrastructure.Repositories.Interfaces;
using Sales.API.Helpers;

namespace Sales.API.Infrastructure.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly SalesDataContex _context;
        public CityRepository(SalesDataContex context) : base(context) => _context = context;

        public async Task<IEnumerable<City>> GetAllAsync(PaginationDto pagination)
        {
            IQueryable<City> queriable = _context.Cities.Where(c => c.StateId == pagination.Id).AsQueryable();

            return await queriable.OrderBy(s => s.Name).Paginate(pagination).ToListAsync();
        }

        public async Task<double> GetPages(PaginationDto pagination)
        {
            IQueryable<City> queryable = _context.Cities.Where(c => c.StateId == pagination.Id).AsQueryable();
            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return totalPages;
        }

        public async Task<ErrorClass> ExistCityInStateAsync(int stateId, string cityName)
        {
            if (!await _context.States.AnyAsync(c => c.Id == stateId))
                return new ErrorClass { Error = true, Message = "El Estado/Departamento no existe" };

            if (await _context.Cities.AnyAsync(s => s.Name == cityName && s.StateId == stateId))
                return new ErrorClass { Error = true, Message = $"La ciudad: {cityName} ya esta registrado para este Estado/Departamento" };

            return new ErrorClass { Error = false, Message = "OK" };
        }
    }
}
