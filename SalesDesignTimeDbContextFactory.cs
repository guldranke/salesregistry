using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sales;

/// <summary>
/// EntityFrameworkCore Design Time DbContext Class
/// <para>
/// Design Time for <see cref="SalesDbContext"/>
/// </para>
/// </summary>
public class SalesDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SalesDbContext> {
    public SalesDbContext CreateDbContext(string[]? args) {
        return new SalesDbContext(new DbContextOptionsBuilder().UseSqlite("Data Source=Sales.db").Options);
    }
}
