using AutoMapper;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sales.API.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpGet("combo/{stateId:int}")]
        public async Task<ActionResult> GetCombo(int stateId)
        {
            IEnumerable<City> getStates = await _cityRepository.GetAllAsync(stateId);
            return Ok(_mapper.Map<IEnumerable<CityDto>>(getStates));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> AddCity(CityDto cityDto)
        {
            cityDto.Id = 0;
            ErrorClass confirData = await _cityRepository.ExistCityInStateAsync(cityDto.StateId, cityDto.Name);
            if (confirData.Error)
                return BadRequest(confirData.Message);

            _cityRepository.Add(_mapper.Map<City>(cityDto));
            return Ok(await _cityRepository.SaveChangesAsync());
        }

        [HttpPut("id")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UpdateState(int id, CityDto cityDto)
        {
            if (!ModelState.IsValid || cityDto.Id != id)
                return BadRequest("Datos invalidos");

            ErrorClass confirData = await _cityRepository.ExistCityInStateAsync(cityDto.StateId, cityDto.Name);
            if (confirData.Error)
                return BadRequest(confirData.Message);

            _cityRepository.Update(_mapper.Map<City>(cityDto));
            return Ok(await _cityRepository.SaveChangesAsync());
        }

        [HttpDelete("id")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteState(int id)
        {
            City city = await _cityRepository.GetByIdAsync(id);
            if (city is null)
                return NotFound();

            _cityRepository.Delete(city);
            return Ok(await _cityRepository.SaveChangesAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDto pagination)
        {
            double get = await _cityRepository.GetPages(pagination);
            return Ok(get);
        }
    }
}
