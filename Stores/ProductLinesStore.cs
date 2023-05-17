using Sales.Models;
using Sales.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Stores;

/// <summary>
/// Class ProductLinesStore is the source of product lines for a <see cref="SalesMan"/>
/// </summary>
public class ProductLinesStore {
    private readonly IGetAllProductLineWithProductQuery getAllProductLinesWithProductQuery;

    private readonly IGetAllProductLineQuery getAllProductLinesQuery;
    private readonly IQuery<ProductLine> createProductLineQuery;
    private readonly IQuery<ProductLine> updateProductLineQuery;
    private readonly IQuery<ProductLine> deleteProductLineQuery;

    private readonly List<ProductLine> productLines;
    public List<ProductLine> ProductLines => productLines;

    public event Action? ProductLinesLoaded;
    public event Action<ProductLine>? ProductLinesCreated;
    public event Action<ProductLine>? ProductLinesUpdated;
    public event Action<ProductLine>? ProductLinesDeleted;

    public ProductLinesStore(
        IGetAllProductLineWithProductQuery getAllProductLinesWithProductQuery,
        IGetAllProductLineQuery getAllProductLinesQuery,
        IQuery<ProductLine> createProductLineQuery,
        IQuery<ProductLine> updateProductLineQuery,
        IQuery<ProductLine> deleteProductLineQuery
    ) {
        this.getAllProductLinesWithProductQuery = getAllProductLinesWithProductQuery;

        this.getAllProductLinesQuery = getAllProductLinesQuery;
        this.createProductLineQuery = createProductLineQuery;
        this.updateProductLineQuery = updateProductLineQuery;
        this.deleteProductLineQuery = deleteProductLineQuery;

        this.productLines = new();
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.GetAllProductLinesQuery"/>
    /// </summary>
    public async Task Load(SalesMan salesMan) {
        IEnumerable<ProductLine> productLines = await this.getAllProductLinesQuery.Execute(salesMan);

        this.productLines.Clear();

        if (!productLines.Any()) {
            this.productLines.Add(new ProductLine(-1, -1, 0, DateTime.Now, 0.00, 0));
        } else {
            this.productLines.AddRange(productLines);
        }

        ProductLinesLoaded?.Invoke();
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.CreateProductLineQuery"/>
    /// </summary>
    public async Task Add(ProductLine productLine) {
        await this.createProductLineQuery.Execute(productLine);

        int index = this.productLines.FindIndex((p) => p.ProdLineId == -1);

        if (index == -1) {
            this.productLines.Add(productLine);
        } else {
            this.productLines[index] = productLine;
        }

        ProductLinesCreated?.Invoke(productLine);
    }

    /// <summary>
    /// Synchronously adds a temporary product line
    /// </summary>
    public void AddTemporary(ProductLine productLine) {
        if (this.productLines.Find((p) => p.ProdLineId == -1) == null) {
            this.productLines.Add(productLine);
            ProductLinesCreated?.Invoke(productLine);
        }
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.UpdateProductLineQuery"/>
    /// </summary>
    public async Task Update(ProductLine productLine) {
        await this.updateProductLineQuery.Execute(productLine);

        int index = this.productLines.FindIndex((p) => p.ProdLineId == productLine.ProdLineId);

        if (index == -1) {
            this.productLines.Add(productLine);
        } else {
            this.productLines[index] = productLine;
        }

        ProductLinesUpdated?.Invoke(productLine);
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.DeleteProductLineQuery"/>
    /// </summary>
    public async Task Delete(ProductLine productLine) {
        if (!productLine.IsTemporary) {
            await this.deleteProductLineQuery.Execute(productLine);
        }

        this.productLines.RemoveAll((p) => p.ProdLineId == productLine.ProdLineId);

        ProductLinesDeleted?.Invoke(productLine);

        if (this.productLines.Count == 0) {
            AddTemporary(new ProductLine(-1, -1, 0, DateTime.Now, 0.00, 0));
        }
    }

    public async Task<IEnumerable<ProductLineWithProduct>> GetProductLineWithProducts(SalesMan salesMan, DateTime startDate, DateTime endDate) {
        return await this.getAllProductLinesWithProductQuery.Execute(salesMan, startDate, endDate);
    }
}
