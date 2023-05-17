using Sales.DTOs;
using Sales.Models;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class DeleteProductQuery deletes a <see cref="Product"/> from the database
/// </summary>
/// <remarks>
/// This isn't used in the code but rather for testing purposes
/// </remarks>
public class DeleteProductQuery : IQuery<Product> {
    private readonly SalesDbContextFactory contextFactory;

    public DeleteProductQuery(SalesDbContextFactory contextFactory) {
        this.contextFactory = contextFactory;
    }

    public async Task Execute(Product product) {
        using SalesDbContext context = this.contextFactory.Create();

        ProductDto productDto = new() {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            Price = product.Price,
        };

        context.Products.Entry(productDto).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        await context.SaveChangesAsync();
    }
}
