using LinqRetrievalLab.Models;
using Microsoft.EntityFrameworkCore;

namespace LinqRetrievalLab.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "School Supplies" },
            new Category { Id = 3, Name = "Accessories" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 35000,
                Stock = 10,
                CategoryId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Mouse",
                Price = 800,
                Stock = 25,
                CategoryId = 3
            },
            new Product
            {
                Id = 3,
                Name = "Keyboard",
                Price = 1500,
                Stock = 15,
                CategoryId = 3
            },
            new Product
            {
                Id = 4,
                Name = "Notebook",
                Price = 100,
                Stock = 50,
                CategoryId = 2
            },
            new Product
            {
                Id = 5,
                Name = "Ballpen",
                Price = 30,
                Stock = 100,
                CategoryId = 2
            }
        );
    }
}