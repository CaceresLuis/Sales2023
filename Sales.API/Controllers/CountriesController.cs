using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IRepository<Country> _countryRepository;

        public CountriesController(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Country>> GetCountries()
        {
            return await _countryRepository.GetAllAsync();
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetCountry(int id)
        {
            Country country = await _countryRepository.GetByIdAsync(id);
            if (country is null)
                return NotFound();

            return Ok(country);
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry(CountryDto countryDto)
        {
            Country country = new()
            {
                Name = countryDto.Name
            };

            return Ok(await _countryRepository.AddAsync(country));
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateCountry(int id, CountryDto countryDto)
        {
            if (!ModelState.IsValid || countryDto.Id != id)
                return BadRequest("Datos invalidos");

            Country country = new()
            {
                Id = countryDto.Id,
                Name = countryDto.Name
            };

            return Ok(await _countryRepository.UpdateAsync(country));
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            Country country = await _countryRepository.GetByIdAsync(id);
            if (country is null)
                return NotFound();

            return Ok(await _countryRepository.DeleteAsync(country));
        }
    }
}
