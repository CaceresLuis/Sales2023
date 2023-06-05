using Sales.API.Data;
using Sales.API.Data.Entities;
using Sales.API.Infrastructure.Repositories.Interfaces;

namespace Sales.API.Infrastructure.Repositories
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(SalesDataContex context) : base(context)
        {
        }
    }
}
