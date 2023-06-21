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
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAllAsync(pagination);
            if (!categories.Any())
                return NotFound("Aun no hay registro de categorias");

            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        [HttpGet("id")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryDto>> GetAsync(int id)
        {
            Category category = await _categoryRepository.GetByIdActiveAsync(id);
            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetCategoriesDeletedAsync")]
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
            Category categoryExist = await _categoryRepository.GetCategoryIfExist(categoryDto.Name);
            if (categoryExist is not null)
            {
                if (categoryExist.IsDeleted)
                    return BadRequest("Esta categoria ya existe como borrada");
                else 
                    return BadRequest("Esta categoria ya existe");
            }

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
            if (category is null)
                return BadRequest($"La categoria: no existe");

            category.UpdateAt = DateTime.UtcNow;
            category.IsUpdated = true;

            if (categoryDto.Restore)
            {
                category.IsDeleted = categoryDto.IsDeleted;
                _categoryRepository.Update(category);
                return Ok(await _categoryRepository.SaveChangesAsync());
            }

            Category categoryExist = await _categoryRepository.GetCategoryIfExist(categoryDto.Name);
            if (categoryExist is not null)
            {
                if (categoryExist.IsDeleted) 
                    return BadRequest("Esta category ya existe como borrada");
                else 
                    return BadRequest("Esta category ya existe");
            }

            
            category.Name = categoryDto.Name;
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
        [AllowAnonymous]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDto pagination, bool deleted)
        {
            return Ok(await _categoryRepository.GetPages(pagination, deleted));
        }
    }
}
