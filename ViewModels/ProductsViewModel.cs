using Sales.Commands;
using Sales.Models;
using Sales.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sales.ViewModels;

/// <summary>
/// Class ProductsViewModel models the <see cref="Views.Products.Products"/> view
/// </summary>
public class ProductsViewModel : BaseViewModel {
    private readonly ObservableCollection<ProductViewModel> products;

    private int SelectedIndex 
        => this.products.IndexOf(this.products.Where((p) => p.ProductId == this.selectedProduct?.ProductId).First());
    public string NavigatorDisplayIndex => $"{SelectedIndex + 1} / {this.products.Count}";

    private Product? selectedProduct;
    public Product? SelectedProduct {
        get => selectedProduct; set {
            selectedProduct = value;
            ProductsFormViewModel = new(value!);

            OnPropertyChanged();
            OnPropertyChanged(nameof(NavigatorDisplayIndex));
            OnPropertyChanged(nameof(ProductsFormViewModel));
        }
    }

    public ProductsFormViewModel? ProductsFormViewModel { get; set; }

    private readonly ProductsStore productStore;
    public List<Product> Products => productStore.Products;

    public ICommand? NewCommand { get; }
    public ICommand? CreateCommand { get; }
    public ICommand? UpdateCommand { get; }

    public ProductsViewModel(ProductsStore productStore) {
        this.products = new();
        this.productStore = productStore;

        NewCommand = new NewProductCommand(productStore);
        CreateCommand = new CreateProductCommand(productStore, this);
        UpdateCommand = new UpdateProductCommand(productStore, this);

        // Listen to product store actions
        this.productStore.ProductsLoaded += ProductStore_ProductsLoaded;
        this.productStore.ProductsCreated += ProductStore_ProductsCreated;
        this.productStore.ProductsUpdated += ProductStore_ProductsUpdated;
    }
    protected override void Dispose() {
        this.productStore.ProductsLoaded -= ProductStore_ProductsLoaded;
        this.productStore.ProductsCreated -= ProductStore_ProductsCreated;
        this.productStore.ProductsUpdated -= ProductStore_ProductsUpdated;

        base.Dispose();
    }

    /// <summary>
    /// On products loaded, add all products to <see cref="products"/> and set the <see cref="selectedProduct"/>
    /// </summary>
    private void ProductStore_ProductsLoaded() {
        this.products.Clear();

        foreach (Product product in this.productStore.Products) {
            this.products.Add(new ProductViewModel(product));
        }

        SelectedProduct = this.productStore.Products.First();
    }

    /// <summary>
    /// On product created, add to <see cref="products"/> and set the <see cref="selectedProduct"/>
    /// </summary>
    /// <param name="product"></param>
    private void ProductStore_ProductsCreated(Product product) {
        ProductViewModel? productViewModel = this.products.FirstOrDefault((p) => p.ProductId == -1);

        if (productViewModel != null) {
            // Remove the temporary product upon creating a new real product  
            int index = this.products.IndexOf(productViewModel);
            this.products[index].Update(product);
        } else {
            this.products.Add(new ProductViewModel(product));
        }

        SelectedProduct = product;
    }

    /// <summary>
    /// On product updated, add to <see cref="products"/> and set the <see cref="selectedProduct"/>
    /// </summary>
    /// <param name="product"></param>
    private void ProductStore_ProductsUpdated(Product product) {
        ProductViewModel? productViewModel =
            this.products.FirstOrDefault((p) => p.ProductId == product.ProductId);

        productViewModel?.Update(product);

        SelectedProduct = product;
    }
}
