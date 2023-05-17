using Sales.DTOs;
using Sales.Models;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class DeleteProductLineQuery deletes a <see cref="ProductLine"/> from the database
/// </summary>
public class DeleteProductLineQuery : IQuery<ProductLine> {
    private readonly SalesDbContextFactory contextFactory;

    public DeleteProductLineQuery(SalesDbContextFactory contextFactory) {
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

        context.ProductLines.Entry(productLineDto).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        await context.SaveChangesAsync();
    }
}
