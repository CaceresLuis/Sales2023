using AutoMapper;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Infrastructure.Repositories.Interfaces;
using Sales.API.Infrastructure.Repositories;
using Sales.API.Services.Interfaces;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public ProductsController(IProductRepository productRepository, IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery] PaginationDto pagination)
        {
            var getAll = await _productRepository.GetAllAsync(pagination);
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(getAll));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            Product product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProduct(int id, SimpleProductDto productDto)
        //{
        //    if (id != productDto.Id || !ModelState.IsValid)
        //        return BadRequest();

        //    Product product = _mapper.Map<Product>(productDto);
        //    return Ok(await _productRepository.UpdateAsync(product));
        //}

        [HttpPost]
        public async Task<ActionResult> PostProduct(SimpleProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(await _productService.AddProductAsync(productDto));
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    Product product = await _productRepository.GetByIdAsync(id);
        //    if (product == null)
        //        return NotFound();

        //    return Ok(await _productRepository.DeleteAsync(product));
        //}

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDto pagination)
        {
            double get = await _productRepository.GetPages(pagination);
            return Ok(get);
        }
    }
}
