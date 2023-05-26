using AutoMapper;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAllAsync(pagination);
            if (!categories.Any())
                return NotFound("Aun no hay registro de categorias");

            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        [HttpGet("id")]
        public async Task<ActionResult<CategoryDto>> GetAsync(int id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddCategorie(CategoryDto categoryDto)
        {
            categoryDto.Id = 0;
            if (await _categoryRepository.CategoryExisteAsysn(categoryDto.Name))
                return BadRequest($"La categoria: {categoryDto.Name} ya esta registrada");

            return Ok(await _categoryRepository.AddAsync(_mapper.Map<Category>(categoryDto)));

        }

        [HttpPut("id")]
        public async Task<ActionResult<bool>> UpdateCategorie(int id, CategoryDto categoryDto)
        {
            if (!ModelState.IsValid || categoryDto.Id != id)
                return BadRequest("Datos invalidos");

            if (await _categoryRepository.CategoryExisteAsysn(categoryDto.Name))
                return BadRequest($"La categoria: {categoryDto.Name} ya esta registrada");

            return Ok(await _categoryRepository.UpdateAsync(_mapper.Map<Category>(categoryDto)));
        }

        [HttpDelete("id")]
        public async Task<ActionResult<bool>> DeleteCategorie(int id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound("La categoria ya no existe");

            return Ok(await _categoryRepository.DeleteAsync(category));
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDto pagination)
        {
            double get = await _categoryRepository.GetPages(pagination);
            return Ok(get);
        }
    }
}
