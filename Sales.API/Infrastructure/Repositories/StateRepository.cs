using Sales.API.Data;
using Sales.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
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
    }
}
