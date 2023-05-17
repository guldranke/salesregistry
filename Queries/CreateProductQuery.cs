using Sales.DTOs;
using Sales.Models;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class CreateProductQuery creates a <see cref="Product"/> which is saved in the Database
/// </summary>
public class CreateProductQuery : IQuery<Product> {
    private readonly SalesDbContextFactory contextFactory;

    public CreateProductQuery(SalesDbContextFactory contextFactory) {
        this.contextFactory = contextFactory;
    }

    public async Task Execute(Product product) {
        using SalesDbContext context = this.contextFactory.Create();

        ProductDto productDto = new() {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            Price = product.Price
        };

        context.Products.Add(productDto);
        await context.SaveChangesAsync();

        product.ProductId = productDto.ProductId;
    }
}
