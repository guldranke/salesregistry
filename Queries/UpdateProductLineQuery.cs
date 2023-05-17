using Sales.DTOs;
using Sales.Models;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class UpdateProductLineQuery updates a <see cref="ProductLine"/> which is saved in the Database
/// </summary>
public class UpdateProductLineQuery : IQuery<ProductLine> {
    private readonly SalesDbContextFactory contextFactory;

    public UpdateProductLineQuery(SalesDbContextFactory contextFactory) { 
        this.contextFactory = contextFactory;
    }

    public async Task Execute(ProductLine productLine) {
        using SalesDbContext context = this.contextFactory.Create();

        ProductLineDto productLineDto = new() {
            ProdLineId = productLine.ProdLineId,
            ProductId = productLine.ProductId,
            SalesManId = productLine.SalesManId,
            SalesDate = productLine.SalesDate,
            Price = productLine.Price,
            Amount = productLine.Amount
        };

        context.ProductLines.Update(productLineDto);
        await context.SaveChangesAsync();
    }
}
