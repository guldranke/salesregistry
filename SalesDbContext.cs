using Microsoft.EntityFrameworkCore;
using Sales.DTOs;

namespace Sales;

/// <summary>
/// EntityFrameworkCore DbContext Class
/// </summary>
public class SalesDbContext : DbContext {
    public DbSet<ProductDto> Products { get; set; }
    public DbSet<ProductLineDto> ProductLines { get; set; }
    public DbSet<SalesManDto> SalesMen { get; set; }


    public SalesDbContext(DbContextOptions options) : base(options) { }
}
