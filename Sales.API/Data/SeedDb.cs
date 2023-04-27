using Sales.API.Data.Entities;

namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly SalesDataContex _context;

        public SeedDb(SalesDataContex context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country { Name = "El Salvador" });
                _context.Countries.Add(new Country { Name = "Costa Rica" });
                _context.Countries.Add(new Country { Name = "Argentina" });
                _context.Countries.Add(new Country { Name = "Brasil" });
                _context.Countries.Add(new Country { Name = "Dinamarca" });
                _context.Countries.Add(new Country { Name = "España" });
                _context.Countries.Add(new Country { Name = "Guatemala" });
                _context.Countries.Add(new Country { Name = "Honduras" });
                _context.Countries.Add(new Country { Name = "Inglaterra" });
                _context.Countries.Add(new Country { Name = "Nicaragua" });
                _context.Countries.Add(new Country { Name = "Panamá " });
                _context.Countries.Add(new Country { Name = "Paraguay" });
                _context.Countries.Add(new Country { Name = "Perú" });
                _context.Countries.Add(new Country { Name = "Uruguay" });
                _context.Countries.Add(new Country { Name = "Colombia" });
                _context.Countries.Add(new Country { Name = "Estados Unidos" });
                
                await _context.SaveChangesAsync();
            }
        }
    }
}
