using Karp5Shop.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Karp5Shop.Server.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(product => product.Price).HasColumnType("decimal(18,2)");
            entity.HasData(
                new Product
                {
                    Id = 1,
                    Name = "Ноутбук Lenovo IdeaPad",
                    Description = "15.6 дюйма, 16 ГБ RAM, SSD 512 ГБ",
                    Price = 72990,
                    Stock = 8
                },
                new Product
                {
                    Id = 2,
                    Name = "Смартфон Samsung Galaxy",
                    Description = "128 ГБ памяти, AMOLED-экран",
                    Price = 41990,
                    Stock = 15
                },
                new Product
                {
                    Id = 3,
                    Name = "Беспроводные наушники",
                    Description = "Bluetooth 5.3, активное шумоподавление",
                    Price = 8990,
                    Stock = 27
                });
        });
    }
}
