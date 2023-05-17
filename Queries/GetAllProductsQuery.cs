using Microsoft.EntityFrameworkCore;
using Sales.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class GetAllProducts gets all <see cref="Product"/>'s from the Database
/// </summary>
public class GetAllProductsQuery : IGetAllQuery<Product> {
    private readonly SalesDbContextFactory contextFactory;

    public GetAllProductsQuery(SalesDbContextFactory contextFactory) {
        this.contextFactory = contextFactory;
    }

    /// <returns>
    /// IEnumerable of all products in the Database
    /// </returns>
    public async Task<IEnumerable<Product>> Execute() {
        using SalesDbContext context = this.contextFactory.Create();

        var products = await context.Products.ToListAsync();

        return products.Select((p) => new Product(p.ProductId, p.ProductName, p.Price));
    }
}
