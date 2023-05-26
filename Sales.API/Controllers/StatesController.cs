using AutoMapper;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Infrastructure.Exceptions;
using Sales.API.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStateRepository _stateRepository;

        public StatesController(IStateRepository stateRepository, IMapper mapper)
        {
            _mapper = mapper;
            _stateRepository = stateRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateDto>>> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            IEnumerable<State> states = await _stateRepository.GetAllAsync(pagination);
            if (!states.Any())
                return NotFound("Aun no hay registro de Estados");

            return Ok(_mapper.Map<IEnumerable<StateDto>>(states));
        }

        [HttpGet("id")]
        public async Task<ActionResult<StateDto>> GetAsync(int id)
        {
            State state = await _stateRepository.GetByIdWhitEstatesAsync(id);
            if (state is null)
                return NotFound();

            return Ok(_mapper.Map<StateDto>(state));
        }

        [AllowAnonymous]
        [HttpGet("combo/{countryId:int}")]
        public async Task<ActionResult> GetCombo(int countryId)
        {
            IEnumerable<State> getStates = await _stateRepository.GetAllAsync(countryId);
            return Ok(_mapper.Map<IEnumerable<SimpleStateDto>>(getStates));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddState(StateDto stateDto)
        {
            stateDto.Id = 0;
            ErrorClass confirData = await _stateRepository.ExistStateInCountry(stateDto.CountryId, stateDto.Name);
            if (confirData.Error)
                return BadRequest(confirData.Message);

            return Ok(await _stateRepository.AddAsync(_mapper.Map<State>(stateDto)));

        }

        [HttpPut("id")]
        public async Task<ActionResult<bool>> UpdateState(int id, StateDto stateDto)
        {
            if (!ModelState.IsValid || stateDto.Id != id)
                return BadRequest("Datos invalidos");

            ErrorClass confirData = await _stateRepository.ExistStateInCountry(stateDto.CountryId, stateDto.Name);
            if (confirData.Error)
                return BadRequest(confirData.Message);

            return Ok(await _stateRepository.UpdateAsync(_mapper.Map<State>(stateDto)));
        }

        [HttpDelete("id")]
        public async Task<ActionResult<bool>> DeleteState(int id)
        {
            State state = await _stateRepository.GetByIdAsync(id);
            if (state is null)
                return NotFound();

            return Ok(await _stateRepository.DeleteAsync(state));
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDto pagination)
        {
            double get = await _stateRepository.GetPages(pagination);
            return Ok(get);
        }
    }
}
