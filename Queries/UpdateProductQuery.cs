using Sales.DTOs;
using Sales.Models;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class UpdateProductQuery updates a <see cref="Product"/> which is saved in the Database
/// </summary>
public class UpdateProductQuery : IQuery<Product> {
    private readonly SalesDbContextFactory contextFactory;

    public UpdateProductQuery(SalesDbContextFactory contextFactory) { 
        this.contextFactory = contextFactory;
    }

    public async Task Execute(Product product) {
        using SalesDbContext context = this.contextFactory.Create();


        ProductDto productDto = new() {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            Price = product.Price,
        };

        context.Products.Update(productDto);
        await context.SaveChangesAsync();
    }
}
