using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public CountriesController(ICountryRepository countryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAllAsync()
        {
            IEnumerable<Country> countries = await _countryRepository.GetAllAsync();
            if (!countries.Any())
                return NotFound("Aun no hay registro de paises");

            return Ok(_mapper.Map<IEnumerable<CountryDto>>(countries));
        }

        [HttpGet("id")]
        public async Task<ActionResult<CountryDto>> GetAsync(int id)
        {
            Country country = await _countryRepository.GetByIdAsync(id);
            if (country is null)
                return NotFound();

            return Ok(country);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddCountry(CountryDto countryDto)
        {
            countryDto.Id = 0;
            if (await _countryRepository.GetCountryByName(countryDto.Name))
                return BadRequest($"El pais: {countryDto.Name} ya esta registrado");

            return Ok(await _countryRepository.AddAsync(_mapper.Map<Country>(countryDto)));

        }

        [HttpPut("id")]
        public async Task<ActionResult<bool>> UpdateCountry(int id, CountryDto countryDto)
        {
            if (!ModelState.IsValid || countryDto.Id != id)
                return BadRequest("Datos invalidos");

            if (await _countryRepository.GetCountryByName(countryDto.Name))
                return BadRequest($"El pais: {countryDto.Name} ya esta registrado");

            return Ok(await _countryRepository.UpdateAsync(_mapper.Map<Country>(countryDto)));
        }

        [HttpDelete("id")]
        public async Task<ActionResult<bool>> DeleteCountry(int id)
        {
            Country country = await _countryRepository.GetByIdAsync(id);
            if (country is null)
                return NotFound();

            return Ok(await _countryRepository.DeleteAsync(country));
        }
    }
}
