using Sales.API.Data;
using Sales.API.Data.Entities;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Infrastructure.Repositories
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        //private readonly SalesDataContex _context;
        public ProductCategoryRepository(SalesDataContex context) : base(context)
        {
            //_context = context;
        }
    }
}
