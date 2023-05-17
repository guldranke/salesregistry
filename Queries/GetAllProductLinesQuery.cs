using Microsoft.EntityFrameworkCore;
using Sales.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class GetAllProductLines gets all <see cref="ProductLine"/>'s belonging to a sales man from the Database
/// </summary>
public class GetAllProductLinesQuery : IGetAllProductLineQuery {
    private readonly SalesDbContextFactory contextFactory;

    public GetAllProductLinesQuery(SalesDbContextFactory contextFactory) {
        this.contextFactory = contextFactory;
    }

    /// <returns>
    /// IEnumerable of all product lines belonging to a sales man
    /// </returns>
    public async Task<IEnumerable<ProductLine>> Execute(SalesMan salesMan) {
        using SalesDbContext context = this.contextFactory.Create();

        var productLines = await context.ProductLines.Where((p) => p.SalesManId == salesMan.SalesManId).ToListAsync();

        return productLines.Select((p) => new ProductLine(
            p.ProdLineId,
            p.ProductId,
            p.SalesManId,
            p.SalesDate,
            p.Price,
            p.Amount
        ));
    }
}
