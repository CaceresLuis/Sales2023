using Sales.Shared.DTOs;

namespace Sales.API.Helpers
{
    public static class QueriableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queriable, PaginationDto pagination)
        {
            return queriable.Skip((pagination.Page - 1) * pagination.RecordsNumber).Take(pagination.RecordsNumber);
        }
    }
}
