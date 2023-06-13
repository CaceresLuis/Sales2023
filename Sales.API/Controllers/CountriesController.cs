using AutoMapper;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            IEnumerable<Country> countries = await _countryRepository.GetAllAsync(pagination);
            if (!countries.Any())
                return NotFound("Aun no hay registro de paises");

            return Ok(_mapper.Map<List<CountryDto>>(countries));
        }

        [HttpGet("id")]
        public async Task<ActionResult<CountryDto>> GetAsync(int id)
        {
            Country country = await _countryRepository.GetByIdAsync(id);
            if (country is null)
                return NotFound();

            return Ok(_mapper.Map<CountryDto>(country));
        }

        [HttpGet("combo")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCombo()
        {
            IEnumerable<Country> getCountries = await _countryRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SimpleCountryDto>>(getCountries));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> AddCountry(SimpleCountryDto countryDto)
        {
            if (await _countryRepository.CountryExisteAsync(countryDto.Name))
                return BadRequest($"El pais: {countryDto.Name} ya esta registrado");

            Country country = _mapper.Map<Country>(countryDto);
            country.CrateAt = DateTime.UtcNow;
            _countryRepository.Add(country);
            return Ok(await _countryRepository.SaveChangesAsync());

        }

        [HttpPut("id")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UpdateCountry(int id, SimpleCountryDto countryDto)
        {
            if (!ModelState.IsValid || countryDto.Id != id)
                return BadRequest("Datos invalidos");

            if (await _countryRepository.CountryExisteAsync(countryDto.Name))
                return BadRequest($"El pais: {countryDto.Name} ya esta registrado");

            Country country = _mapper.Map<Country>(countryDto);
            country.UpdateAt = DateTime.UtcNow;
            country.IsUpdated = true;
            _countryRepository.Update(country);
            return Ok(await _countryRepository.SaveChangesAsync());
        }

        [HttpDelete("id")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteCountry(int id)
        {
            Country country = await _countryRepository.GetByIdAsync(id);
            if (country is null)
                return NotFound();

            country.DeleteAt = DateTime.UtcNow;
            country.IsDeleted = true;
            _countryRepository.Update(country);
            return Ok(await _countryRepository.SaveChangesAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDto pagination)
        {
            double get = await _countryRepository.GetPages(pagination);
            return Ok(get);
        }
    }
}
