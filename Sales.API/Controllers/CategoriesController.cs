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
            Category category = await _categoryRepository.GetByIdActiveAsync(id);
            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("DeletedCategories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllDeletedAsync([FromQuery] PaginationDto pagination)
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAllDeletedAsync(pagination);
            if (!categories.Any())
                return NotFound("Aun no hay registro de borradas categorias");

            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> AddCategorie(CategoryDto categoryDto)
        {
            categoryDto.Id = 0;
            if (await _categoryRepository.CategoryExisteAsysn(categoryDto.Name))
                return BadRequest($"La categoria: {categoryDto.Name} ya esta registrada");

            Category category = _mapper.Map<Category>(categoryDto);
            category.CrateAt = DateTime.UtcNow;
            _categoryRepository.Add(category);

            return Ok(await _categoryRepository.SaveChangesAsync());
        }

        [HttpPut("id")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UpdateCategorie(int id, CategoryDto categoryDto)
        {
            if (!ModelState.IsValid || categoryDto.Id != id)
                return BadRequest("Datos invalidos");

            Category category = await _categoryRepository.GetByIdAsync(id);
            if (category.Name == categoryDto.Name)
                return BadRequest($"La categoria: {categoryDto.Name} ya esta registrada");

            category.UpdateAt = DateTime.UtcNow;
            category.IsUpdated = true;
            _categoryRepository.Update(category);

            return Ok(await _categoryRepository.SaveChangesAsync());
        }

        [HttpDelete("id")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteCategorie(int id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound("La categoria ya no existe");

            category.DeleteAt = DateTime.UtcNow;
            category.IsDeleted = true;
            _categoryRepository.Update(category);

            return Ok(await _categoryRepository.SaveChangesAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDto pagination)
        {
            double get = await _categoryRepository.GetPages(pagination);
            return Ok(get);
        }
    }
}
