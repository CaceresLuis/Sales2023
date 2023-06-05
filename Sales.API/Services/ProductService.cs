﻿using AutoMapper;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Sales.API.Services.Interfaces;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductService(IFileStorage fileStorage, IProductRepository productRepository, IProductImageRepository productImageRepository, IProductCategoryRepository productCategoryRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _fileStorage = fileStorage;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productImageRepository = productImageRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<bool> AddProductAsync(SimpleProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);

            if (productDto.ProductImages.Any())
            {
                foreach (string image in productDto.ProductImages)
                {
                    string filePaht = $"{Environment.CurrentDirectory}\\Helpers\\products\\{image}";
                    byte[] fileBytes = File.ReadAllBytes(filePaht);
                    string imagePaht = await _fileStorage.SaveFileAsync(fileBytes, ".jpg", "products");
                    ProductImage addImage = new() { Image = imagePaht, Product = product };
                    await _productImageRepository.AddAsync(addImage);
                }
            }

            if (productDto.ProductCategoriesId.Any())
            {
                foreach (int id in productDto.ProductCategoriesId)
                {
                    Category category = await _categoryRepository.GetByIdAsync(id);
                    if (category != null)
                    {
                        ProductCategory productCategory = new() { Category = category, Product = product };
                        await _productCategoryRepository.AddAsync(productCategory);
                    }
                }
            }
            //return await _productRepository.AddAsync(product);
            return true;
        }
    }
}
