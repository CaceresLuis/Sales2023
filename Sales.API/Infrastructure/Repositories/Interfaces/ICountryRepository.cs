using Sales.API.Data.Entities;

namespace Sales.API.Infrastructure.Repositories.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<bool> CountryExisteAsync(string name);
        Task<IEnumerable<Country>> GetAllCountriesFullDataAsync();
        Task<IEnumerable<Country>> GetAllCountriesWhitEstatesAsync();
    }
}
