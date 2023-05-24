using Microsoft.EntityFrameworkCore;
using Sales.Models;
using Sales.Queries;
using Sales.Stores;
using Sales.ViewModels;
using Sales.Views;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Sales;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    private readonly SelectedSalesManStore selectedSalesManStore;
    private readonly SalesMenStore salesMenStore;
    private readonly ProductsStore productStore;
    private readonly ProductLinesStore productLinesStore;

    private readonly SalesDbContextFactory contextFactory;

    private readonly IGetAllQuery<SalesMan> getAllSalesMenQuery;
    private readonly IQuery<SalesMan> createSalesManQuery;
    private readonly IQuery<SalesMan> updateSalesManQuery;
    private readonly IQuery<SalesMan> deleteSalesManQuery;

    private readonly IGetAllQuery<Product> getAllProductsQuery;
    private readonly IQuery<Product> createProductQuery;
    private readonly IQuery<Product> updateProductQuery;

    private readonly IGetAllProductLineQuery getAllProductLinesQuery;
    private readonly IQuery<ProductLine> createProductLineQuery;
    private readonly IQuery<ProductLine> updateProductLineQuery;
    private readonly IQuery<ProductLine> deleteProductLineQuery;

    private readonly IGetAllProductLineWithProductQuery getallProductLineWithProductQuery;

    public App() {
        FrameworkElement.LanguageProperty.OverrideMetadata(
          typeof(FrameworkElement),
          new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        DbContextOptions options = new DbContextOptionsBuilder().UseSqlite("Data Source=Sales.db").Options;
        contextFactory = new SalesDbContextFactory(options);

        // Ready all queries for SalesMenStore requiring DbContext 
        this.getAllSalesMenQuery = new GetAllSalesMenQuery(this.contextFactory);
        this.createSalesManQuery = new CreateSalesManQuery(this.contextFactory);
        this.updateSalesManQuery = new UpdateSalesManQuery(this.contextFactory);
        this.deleteSalesManQuery = new DeleteSalesManQuery(this.contextFactory);

        this.salesMenStore = new(
            this.getAllSalesMenQuery,
            this.createSalesManQuery,
            this.updateSalesManQuery,
            this.deleteSalesManQuery
        );

        // Ready all queries for ProductsStore requiring DbContext 
        this.getAllProductsQuery = new GetAllProductsQuery(this.contextFactory);
        this.createProductQuery = new CreateProductQuery(this.contextFactory);
        this.updateProductQuery = new UpdateProductQuery(this.contextFactory);

        this.productStore = new(
            this.getAllProductsQuery,
            this.createProductQuery,
            this.updateProductQuery
        );

        // Ready all queries for ProductLinesStore requiring DbContext 
        this.getAllProductLinesQuery = new GetAllProductLinesQuery(contextFactory);
        this.createProductLineQuery = new CreateProductLineQuery(contextFactory);
        this.updateProductLineQuery = new UpdateProductLineQuery(contextFactory);
        this.deleteProductLineQuery = new DeleteProductLineQuery(contextFactory);

        this.getallProductLineWithProductQuery = new GetAllProductLinesWithProductQuery(contextFactory);

        this.productLinesStore = new(
            this.getallProductLineWithProductQuery,
            this.getAllProductLinesQuery,
            this.createProductLineQuery,
            this.updateProductLineQuery,
            this.deleteProductLineQuery
        );

        this.selectedSalesManStore = new(this.salesMenStore);
    }

    protected override void OnStartup(StartupEventArgs e) {
        // Migrate DB on startup
        DbContextOptions options = new DbContextOptionsBuilder().UseSqlite("Data Source=Sales.db").Options;
        using SalesDbContext context = new SalesDbContextFactory(options).Create();

        context.Database.Migrate();

        // Initialize and show MainWindow
        MainWindow mainWindow = new() {
            DataContext = new MainWindowViewModel(this.selectedSalesManStore, this.salesMenStore, this.productStore, this.productLinesStore)
        };

        mainWindow.Show();

        base.OnStartup(e);
    }
}
