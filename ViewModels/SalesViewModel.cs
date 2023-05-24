using Sales.Commands;
using Sales.Models;
using Sales.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sales.ViewModels;

public enum SearchType {
    ProductName,
    SalesDate,
}

/// <summary>
/// Class SalesViewModel models the <see cref="Views.Sales.Sales"/> view
/// </summary>
public class SalesViewModel : BaseViewModel {
    private readonly SelectedSalesManStore selectedSalesManStore;
    private readonly ProductLinesStore productLinesStore;
    private readonly ProductsStore productsStore;

    public IEnumerable<Product> Products => this.productsStore.Products;
    public List<ProductLine> ProductLines => this.productLinesStore.ProductLines;

    private readonly ObservableCollection<ProductLineViewModel> productLines;
    public string NavigatorDisplayIndex => $"{GetSelectedIndex() + 1} / {this.productLines.Count}";

    private ProductLine? selectedProductLine;
    public ProductLine? SelectedProductLine {
        get => selectedProductLine; set {
            selectedProductLine = value;
            SalesFormViewModel = new(value!, this.productsStore.Products);

            OnPropertyChanged();
            OnPropertyChanged(nameof(NavigatorDisplayIndex));
            OnPropertyChanged(nameof(SalesFormViewModel));
        }
    }

    public SalesFormViewModel? SalesFormViewModel { get; set; }

    public SearchType SearchMode { get; set; } = SearchType.ProductName;
    public string SearchQuery { get; set; } = string.Empty;

    public ICommand? NewCommand { get; }
    public ICommand? CreateCommand { get; }
    public ICommand? UpdateCommand { get; }
    public ICommand? DeleteCommand { get; }

    public SalesViewModel(SelectedSalesManStore selectedSalesManStore, ProductLinesStore productLinesStore, ProductsStore productsStore) {
        this.selectedSalesManStore = selectedSalesManStore;
        this.productLinesStore = productLinesStore;
        this.productsStore = productsStore;

        this.productLines = new();

        NewCommand = new NewProductLineCommand(productLinesStore);
        CreateCommand = new CreateProductLineCommand(productLinesStore, this, selectedSalesManStore);
        UpdateCommand = new UpdateProductLineCommand(productLinesStore, this, selectedSalesManStore);
        DeleteCommand = new DeleteProductLineCommand(productLinesStore, this);

        this.productsStore.ProductsLoaded += ProductsStore_ProductsLoaded;

        // Listen to product line store actions
        this.productLinesStore.ProductLinesLoaded += ProductLinesStore_ProductLinesLoaded;
        this.productLinesStore.ProductLinesCreated += ProductLinesStore_ProductLinesCreated;
        this.productLinesStore.ProductLinesUpdated += ProductLinesStore_ProductLinesUpdated;
        this.productLinesStore.ProductLinesDeleted += ProductLinesStore_ProductLinesDeleted;

        this.selectedSalesManStore.SelectedSalesManChanged += SelectedSalesManStore_SelectedSalesManChanged;
    }

    /// <returns>
    /// Index of selected product line. Returns -1 if not found
    /// </returns>
    private int GetSelectedIndex() {
        if (this.selectedProductLine == null) return -1;

        IEnumerable<ProductLineViewModel> selected = 
            this.productLines.Where((p) => p.ProdLineId == this.selectedProductLine.ProdLineId);

        if (!selected.Any()) return -1;

        return this.productLines.IndexOf(selected.First());
    }

    protected override void Dispose() {
        this.productsStore.ProductsLoaded -= ProductsStore_ProductsLoaded;

        this.productLinesStore.ProductLinesLoaded -= ProductLinesStore_ProductLinesLoaded;
        this.productLinesStore.ProductLinesCreated -= ProductLinesStore_ProductLinesCreated;
        this.productLinesStore.ProductLinesUpdated -= ProductLinesStore_ProductLinesUpdated;
        this.productLinesStore.ProductLinesDeleted -= ProductLinesStore_ProductLinesDeleted;

        this.selectedSalesManStore.SelectedSalesManChanged -= SelectedSalesManStore_SelectedSalesManChanged;

        base.Dispose();
    }

    /// <summary>
    /// For updating SalesFormViewModel
    /// </summary>
    private void ProductsStore_ProductsLoaded() {
        SelectedProductLine = this.selectedProductLine;
    }

    /// <summary>
    /// On product lines loaded, add all products to <see cref="productLines"/> and set the <see cref="selectedProductLine"/>
    /// </summary>
    private void ProductLinesStore_ProductLinesLoaded() {
        this.productLines.Clear();

        foreach (ProductLine line in this.productLinesStore.ProductLines) {
            this.productLines.Add(new ProductLineViewModel(line));
        }

        SelectedProductLine = this.productLinesStore.ProductLines.First();
    }

    /// <summary>
    /// On product created, add to <see cref="productLines"/> and set the <see cref="selectedProductLine"/>
    /// </summary>
    /// <param name="productLine"></param>
    private void ProductLinesStore_ProductLinesCreated(ProductLine productLine) {
        ProductLineViewModel? productLineViewModel = this.productLines.FirstOrDefault((p) => p.ProdLineId == -1);

        if (productLineViewModel != null && !productLine.IsTemporary) {
            // Remove the temporary product line upon creating a new real product line  
            int index = this.productLines.IndexOf(productLineViewModel);
            this.productLines[index].Update(productLine);
        } else {
            this.productLines.Add(new ProductLineViewModel(productLine));
        }

        SelectedProductLine = productLine;
    }

    /// <summary>
    /// On product updated, add to <see cref="productLines"/> and set the <see cref="selectedProductLine"/>
    /// </summary>
    /// <param name="productLine"></param>
    private void ProductLinesStore_ProductLinesUpdated(ProductLine productLine) {
        ProductLineViewModel? productLineViewModel =
            this.productLines.FirstOrDefault((p) => p.ProdLineId == productLine.ProdLineId);

        productLineViewModel?.Update(productLine);

        SelectedProductLine = productLine;
    }

    /// <summary>
    /// On product deleted, remove from <see cref="productLines"/> and set the <see cref="selectedProductLine"/>
    /// </summary>
    /// <param name="productLine"></param>
    private void ProductLinesStore_ProductLinesDeleted(ProductLine productLine) {
        ProductLineViewModel? productLineViewModel = 
            this.productLines.FirstOrDefault((p) => p.ProdLineId == productLine.ProdLineId);

        if (productLineViewModel != null) {
            this.productLines.Remove(productLineViewModel);
            SelectedProductLine = this.productLinesStore.ProductLines.First();
        }
    }

    /// <summary>
    /// On selected sales man change, get all product lines belonging to sales man
    /// </summary>
    private async void SelectedSalesManStore_SelectedSalesManChanged() {
        if (this.selectedSalesManStore.SelectedSalesMan == null) return;

        await this.productLinesStore.Load(this.selectedSalesManStore.SelectedSalesMan);
    }
}
