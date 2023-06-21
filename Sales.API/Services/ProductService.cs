using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Sales.API.Services.Interfaces;
using Sales.API.Infrastructure.Exceptions;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IFileStorage _fileStorage;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductService(IFileStorage fileStorage, IProductRepository productRepository, IProductImageRepository productImageRepository, IProductCategoryRepository productCategoryRepository, ICategoryRepository categoryRepository)
        {
            _fileStorage = fileStorage;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productImageRepository = productImageRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<Product> GetByIdActiveAsync(int id) => await _productRepository.GetByIdActiveAsync(id);

        public async Task<Product> GetDeleteByIdAsync(int id) => await _productRepository.GetDeleteByIdAsync(id);

        public async Task<double> GetPages(PaginationDto pagination) => await _productRepository.GetPages(pagination);

        public async Task<IEnumerable<Product>> GetAllAsync(PaginationDto pagination) => await _productRepository.GetAllAsync(pagination);

        public async Task<IEnumerable<Product>> GetAllDeltedAsync(PaginationDto pagination) => await _productRepository.GetAllDeltedAsync(pagination);

        public async Task<CustomResponse> AddProductAsync(SimpleProductDto productDto)
        {
            Product product = new()
            {
                Description = productDto.Description,
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };

            if (await _productRepository.NameProductExistAsync(product.Name))
                return new CustomResponse { Error = "El producto ya existe", Succeeded = false };

            if (productDto.ProductImages != null)
                await AddImageProductAsync(productDto.ProductImages, product);

            if (productDto.ProductCategoriesId != null)
                await AddProductCatgoryAsync(productDto.ProductCategoriesId, product);

            product.CrateAt = DateTime.UtcNow;
            _productRepository.Add(product);

            if (!await _productCategoryRepository.SaveChangesAsync())
                return new CustomResponse { Error = "Algo ha ido mal, vulve a intntar mas tarde", Succeeded = false };

            return new CustomResponse { Succeeded = true };
        }

        public async Task<CustomResponse> UpdateProductAsync(SimpleProductDto productDto)
        {
            Product product = await _productRepository.GetByIdAsync(productDto.Id);

            product.IsUpdated = true;
            product.Stock = productDto.Stock;
            product.Price = productDto.Price;
            product.UpdateAt = DateTime.UtcNow;
            product.Name = productDto.Name ?? productDto.Name;
            product.Description = productDto.Description ?? product.Description;

            if (productDto.ProductImages != null)
                await AddImageProductAsync(productDto.ProductImages, product);

            if (productDto.ProductCategoriesId != null)
                await AddProductCatgoryAsync(productDto.ProductCategoriesId, product);

            _productRepository.Update(product);

            if (!await _productRepository.SaveChangesAsync())
                return new CustomResponse { Succeeded = false, Error = "Algo ha salido mal" };

            return new CustomResponse { Succeeded = true };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            Product product = await _productRepository.GetByIdAsync(id);
            product.IsDeleted = true;
            product.DeleteAt = DateTime.UtcNow;

            _productRepository.Update(product);
            
            return await _productRepository.SaveChangesAsync();
        }

        private async Task AddImageProductAsync(List<string> images, Product product)
        {
            foreach (string image in images)
            {
                var photoProduct = Convert.FromBase64String(image);
                string imagePaht = await _fileStorage.SaveFileAsync(photoProduct, ".jpg", "products");
                ProductImage addImage = new() { Image = imagePaht, Product = product };
                product.ProductImages.Add(addImage);
                _productImageRepository.Add(addImage);
            }
        }

        private async Task AddProductCatgoryAsync(List<int> catgoriesId, Product product)
        {
            foreach (int categoryId in catgoriesId)
            {
                Category category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {
                    ProductCategory productCategory = new() { Category = category, Product = product };
                    product.ProductCategories.Add(productCategory);
                    _productCategoryRepository.Add(productCategory);
                }
            }
        }
    }
}
