using Sales.Models;
using Sales.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Stores;

/// <summary>
/// Class ProductsStore is the single source of all products for the application
/// </summary>
public class ProductsStore {
    private readonly IGetAllQuery<Product> getAllProductsQuery;
    private readonly IQuery<Product> createProductQuery;
    private readonly IQuery<Product> updateProductQuery;

    private readonly List<Product> products;
    public List<Product> Products => products;

    public event Action? ProductsLoaded;
    public event Action<Product>? ProductsCreated;
    public event Action<Product>? ProductsUpdated;

    public ProductsStore(
        IGetAllQuery<Product> getAllProductsQuery,
        IQuery<Product> createProductQuery,
        IQuery<Product> updateProductQuery) {

        this.getAllProductsQuery = getAllProductsQuery;
        this.createProductQuery = createProductQuery;
        this.updateProductQuery = updateProductQuery;

        products = new();
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.GetAllProductsQuery"/>
    /// </summary>
    public async Task Load() {
        IEnumerable<Product> products = await this.getAllProductsQuery.Execute();

        this.products.Clear();

        if (!products.Any()) {
            this.products.Add(new Product(-1, "", 0));
        } else {
            this.products.AddRange(products);
        }

        ProductsLoaded?.Invoke();
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.CreateProductQuery"/>
    /// </summary>
    public async Task Add(Product product) {
        await this.createProductQuery.Execute(product);

        int index = this.products.FindIndex((p) => p.ProductId == -1);

        if (index == -1) {
            this.products.Add(product);
        } else {
            this.products[index] = product;
        }

        ProductsCreated?.Invoke(product);
    }

    /// <summary>
    /// Synchronously adds a temporary product
    /// </summary>
    public void AddTemporary(Product product) {
        if (this.products.Find((p) => p.ProductId == -1) == null) {
            this.products.Add(product);
            ProductsCreated?.Invoke(product);
        }
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.UpdateProductQuery"/>
    /// </summary>
    public async Task Update(Product product) {
        await this.updateProductQuery.Execute(product);

        int index = this.products.FindIndex((p) => p.ProductId == product.ProductId);

        if (index == -1) {
            this.products.Add(product);
        } else {
            this.products[index] = product;
        }

        ProductsUpdated?.Invoke(product);
    }
}
