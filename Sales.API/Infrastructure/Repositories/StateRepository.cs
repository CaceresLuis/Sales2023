using Sales.API.Data;
using Sales.Shared.DTOs;
using Sales.API.Helpers;
using Sales.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Sales.API.Infrastructure.Exceptions;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Infrastructure.Repositories
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        private readonly SalesDataContex _context;
        public StateRepository(SalesDataContex context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<State>> GetAllAsync(int countryId) => await _context.States.Where(s => s.CountryId == countryId).ToListAsync();

        public async Task<State> GetByIdWhitEstatesAsync(int id) => await _context.States.Include(s => s.Cities).FirstOrDefaultAsync(s => s.Id == id);

        public async Task<IEnumerable<State>> GetAllAsync(PaginationDto pagination)
        {
            IQueryable<State> queriable = _context.States.Include(s => s.Cities)
                .Where(s => s.CountryId == pagination.Id).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
                queriable = queriable.Where(c => c.Name.ToLower().Contains(pagination.Filter.ToLower()));

            return await queriable.OrderBy(s => s.Name).Paginate(pagination).ToListAsync();
        }

        public async Task<ErrorClass> ExistStateInCountry(int countryId, string nameState)
        {
            if (!await _context.Countries.AnyAsync(c => c.Id == countryId))
                return new ErrorClass { Error = true, Message = "El pais no existe"};

            if(await _context.States.AnyAsync(s => s.Name == nameState && s.CountryId == countryId))
                return new ErrorClass { Error = true, Message = $"El Estado/Departamento {nameState} ya esta registrado para este pais" };
            
            return new ErrorClass { Error = false, Message = "OK" };
        }

        public async Task<double> GetPages(PaginationDto pagination)
        {
            IQueryable<State> queriable = _context.States.Where(s => s.CountryId == pagination.Id).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
                queriable = queriable.Where(c => c.Name.ToLower().Contains(pagination.Filter.ToLower()));

            double count = await queriable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return totalPages;
        }
    }
}
