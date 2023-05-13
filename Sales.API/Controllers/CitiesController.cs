using AutoMapper;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Infrastructure.Exceptions;
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
        public async Task<ActionResult<IEnumerable<CityDto>>> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            IEnumerable<City> city = await _cityRepository.GetAllAsync(pagination);
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
            cityDto.Id = 0;
            ErrorClass confirData = await _cityRepository.ExistCityInStateAsync(cityDto.StateId, cityDto.Name);
            if (confirData.Error)
                return BadRequest(confirData.Message);

            return Ok(await _cityRepository.AddAsync(_mapper.Map<City>(cityDto)));
        }

        [HttpPut("id")]
        public async Task<ActionResult<bool>> UpdateState(int id, CityDto cityDto)
        {
            if (!ModelState.IsValid || cityDto.Id != id)
                return BadRequest("Datos invalidos");

            ErrorClass confirData = await _cityRepository.ExistCityInStateAsync(cityDto.StateId, cityDto.Name);
            if (confirData.Error)
                return BadRequest(confirData.Message);

            return Ok(await _cityRepository.UpdateAsync(_mapper.Map<City>(cityDto)));
        }

        [HttpDelete("id")]
        public async Task<ActionResult<bool>> DeleteState(int id)
        {
            City city = await _cityRepository.GetByIdAsync(id);
            if (city is null)
                return NotFound();

            return Ok(await _cityRepository.DeleteAsync(city));
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDto pagination)
        {
            double get = await _cityRepository.GetPages(pagination);
            return Ok(get);
        }
    }
}
