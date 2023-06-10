using AutoMapper;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Services.Interfaces;
using Sales.API.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Sales.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery] PaginationDto pagination)
        {
            IEnumerable<Product> productos = await _productService.GetAllAsync(pagination);
            if (!productos.Any())
                return NotFound("Aun no hay productos");

            return Ok(_mapper.Map<IEnumerable<ProductDto>>(productos));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("DeletedProducts")]
        public async Task<ActionResult> GetDeletedProducts([FromQuery] PaginationDto pagination)
        {
            IEnumerable<Product> productos = await _productService.GetAllDeltedAsync(pagination);
            if (!productos.Any())
                return NotFound("Aun no hay productos borrados");

            return Ok(_mapper.Map<IEnumerable<ProductDto>>(productos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            Product product = await _productService.GetByIdActiveAsync(id);
            if (product == null)
                return NotFound("Producto no encontrado");

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("DeleteProdut/{id}")]
        public async Task<ActionResult> GetDeleteProduct(int id)
        {
            Product product = await _productService.GetDeleteByIdAsync(id);
            if (product == null)
                return NotFound("Producto no encontrado");

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PostProduct(SimpleProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            CustomResponse add = await _productService.AddProductAsync(productDto);
            if (!add.Succeeded)
                return BadRequest(add.Error);

            return Ok(add.Succeeded);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutProduct(int id, SimpleProductDto productDto)
        {
            if (id != productDto.Id || !ModelState.IsValid)
                return BadRequest();

            var response = await _productService.UpdateProductAsync(productDto);
            if(!response.Succeeded)
                return BadRequest(response.Error);

            return Ok(response.Succeeded);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return Ok(await _productService.DeleteProductAsync(id));
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDto pagination)
        {
            return Ok(await _productService.GetPages(pagination));
        }
    }
}
