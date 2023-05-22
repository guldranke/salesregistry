using Microsoft.EntityFrameworkCore;
using Sales.DTOs;
using Sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class GetAllProductLinesWithProductQuery gets all <see cref="ProductLineWithProduct"/>'s belonging to a sales man from the Database
/// </summary>
public class GetAllProductLinesWithProductQuery : IGetAllProductLineWithProductQuery {
    private readonly SalesDbContextFactory contextFactory;

    public GetAllProductLinesWithProductQuery(SalesDbContextFactory contextFactory) {
        this.contextFactory = contextFactory;
    }

    /// <returns>
    /// IEnumerable of all product lines with product belonging to a sales man within 2 dates
    /// </returns>
    public async Task<IEnumerable<ProductLineWithProduct>> Execute(SalesMan salesMan, DateTime startDate, DateTime endDate) {
        using SalesDbContext context = this.contextFactory.Create();

        List<ProductLineDto> productLines =
            await context.ProductLines
            .Where((p) => p.SalesManId == salesMan.SalesManId)
            .Where((p) => p.SalesDate.Date >= startDate.Date && p.SalesDate.Date <= endDate.Date)
            .ToListAsync();

        var productLinesWithProduct = productLines.Select(async (p) => {
            string productName = "?";
            try {
                ProductDto product = await context.Products.Where((product) => product.ProductId == p.ProductId).SingleAsync();
                productName = product.ProductName;
            } catch { }
            
            return new ProductLineWithProduct(
                    p.ProdLineId,
                    p.ProductId,
                    p.SalesManId,
                    p.SalesDate,
                    p.Price,
                    p.Amount,
                    productName
                );
        });

        return await Task.WhenAll(productLinesWithProduct);
    }
}
