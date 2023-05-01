using AutoMapper;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICityRepository _cityRepository;

        public CitiesController(ICityRepository cityRepository, IMapper mapper)
        {
            _mapper = mapper;
            _cityRepository = cityRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetAllAsync()
        {
            IEnumerable<City> city = await _cityRepository.GetAllAsync();
            if (!city.Any())
                return NotFound("Aun no hay registro de Ciudades");

            return Ok(_mapper.Map<IEnumerable<CityDto>>(city));
        }

        [HttpGet("id")]
        public async Task<ActionResult<CityDto>> GetAsync(int id)
        {
            City city = await _cityRepository.GetByIdAsync(id);
            if (city is null)
                return NotFound();

            return Ok(_mapper.Map<CityDto>(city));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddCity(CityDto cityDto)
        {
            try
            {
                cityDto.Id = 0;
                return Ok(await _cityRepository.AddAsync(_mapper.Map<City>(cityDto)));
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (e.InnerException.Message.Contains("dupicate"))
                    return BadRequest($"La ciudad: {cityDto.Name} ya esta registrado para este Estado");

                return BadRequest();
            }
        }

        [HttpPut("id")]
        public async Task<ActionResult<bool>> UpdateState(int id, CityDto cityDto)
        {
            try
            {
                if (!ModelState.IsValid || cityDto.Id != id)
                    return BadRequest("Datos invalidos");

                return Ok(await _cityRepository.UpdateAsync(_mapper.Map<City>(cityDto)));
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (e.InnerException.Message.Contains("dupicate"))
                    return BadRequest($"La ciudad: {cityDto.Name} ya esta registrado para este Estado");

                return BadRequest();
            }
        }

        [HttpDelete("id")]
        public async Task<ActionResult<bool>> DeleteState(int id)
        {
            City city = await _cityRepository.GetByIdAsync(id);
            if (city is null)
                return NotFound();

            return Ok(await _cityRepository.DeleteAsync(city));
        }
    }
}
