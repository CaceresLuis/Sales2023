using Sales.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Sales.API.Data
{
    public class SalesDataContex : DbContext
    {
        public SalesDataContex(DbContextOptions<SalesDataContex> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
        }
    }
}
