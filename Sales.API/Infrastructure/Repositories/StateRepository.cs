using Sales.API.Data;
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

        public async Task<IEnumerable<State>> GetAllWhitEstatesAsync()
        {
            return await _context.States
                .Include(s => s.Cities)
                .ToListAsync();
        }

        public async Task<State> GetByIdWhitEstatesAsync(int id)
        {
            return await _context.States
                .Include(s => s.Cities)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public new async Task<bool> UpdateAsync(State state)
        {
            _context.Update(state);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ErrorClass> ExistStateInCountry(int countryId, string nameState)
        {
            if (!await _context.Countries.AnyAsync(c => c.Id == countryId))
                return new ErrorClass { Error = true, Message = "El pais no existe"};

            if(await _context.States.AnyAsync(s => s.Name == nameState && s.CountryId == countryId))
                return new ErrorClass { Error = true, Message = $"El Estado/Departamento {nameState} ya esta registrado para este pais" };
            
            return new ErrorClass { Error = false, Message = "OK" };
        }
    }
}
