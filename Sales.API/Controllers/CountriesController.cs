﻿using AutoMapper;
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
        public async Task<IEnumerable<CountryDto>> GetAllAsync()
        {
            IEnumerable<Country> countries = await _countryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CountryDto>>(countries);
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
            Country country = new()
            {
                Name = countryDto.Name
            };

            return Ok(await _countryRepository.AddAsync(country));
        }

        [HttpPut("id")]
        public async Task<ActionResult<bool>> UpdateCountry(int id, CountryDto countryDto)
        {
            if (!ModelState.IsValid || countryDto.Id != id)
                return BadRequest("Datos invalidos");

            if(await _countryRepository.GetCountryByName(countryDto.Name))
                return BadRequest($"El pais: {countryDto.Name} ya esta registrado");

            Country country = _mapper.Map<Country>(countryDto);

            return Ok(await _countryRepository.UpdateAsync(country));
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
