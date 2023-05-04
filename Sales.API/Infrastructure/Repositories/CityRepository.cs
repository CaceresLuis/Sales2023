using Sales.API.Data;
using Sales.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Sales.API.Infrastructure.Exceptions;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Infrastructure.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly SalesDataContex _context;
        public CityRepository(SalesDataContex context) : base(context) => _context = context;

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
