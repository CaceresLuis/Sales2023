﻿using Sales.API.Data;
using Sales.Shared.DTOs;
using Sales.API.Helpers;
using Sales.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly SalesDataContex _context;
        public CategoryRepository(SalesDataContex context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(PaginationDto pagination)
        {
            IQueryable<Category> queriable = _context.Categories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
                queriable = queriable.Where(c => c.Name.ToLower().Contains(pagination.Filter.ToLower()) && !c.IsDeleted);

            return await queriable.OrderBy(c => c.Name).Paginate(pagination).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllDeletedAsync(PaginationDto pagination)
        {
            IQueryable<Category> queriable = _context.Categories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
                queriable = queriable.Where(c => c.Name.ToLower().Contains(pagination.Filter.ToLower()) && c.IsDeleted);

            return await queriable.OrderBy(c => c.Name).Paginate(pagination).ToListAsync();
        }

        public async Task<double> GetPages(PaginationDto pagination)
        {
            IQueryable<Category> queriable = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
                queriable = queriable.Where(c => c.Name.ToLower().Contains(pagination.Filter.ToLower()) && !c.IsDeleted);

            double count = await queriable.CountAsync();
            return Math.Ceiling(count / pagination.RecordsNumber);
        }

        public async Task<bool> CategoryExisteAsysn(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name == name);
        }
    }
}
