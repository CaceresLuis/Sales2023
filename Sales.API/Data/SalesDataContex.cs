using Sales.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Sales.API.Data
{
    public class SalesDataContex : DbContext
    {
        public SalesDataContex(DbContextOptions<SalesDataContex> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
        }
    }
}
