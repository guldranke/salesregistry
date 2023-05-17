using Microsoft.EntityFrameworkCore;

namespace Sales;

/// <summary>
/// EntityFrameworkCore DbContextFactory Class
/// <para>
/// Factory for <see cref="SalesDbContext"/>
/// </para>
/// </summary>
public class SalesDbContextFactory {
    private readonly DbContextOptions options;

    public SalesDbContextFactory(DbContextOptions options) {
        this.options = options;
    }

    public SalesDbContext Create() {
        return new SalesDbContext(this.options);
    }
}
