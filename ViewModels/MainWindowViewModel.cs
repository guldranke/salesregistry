using Sales.Commands;
using Sales.Stores;

namespace Sales.ViewModels;

/// <summary>
/// Class MainWindowViewModel models the <see cref="Views.MainWindow.MainWindow"/>
/// </summary>
public class MainWindowViewModel : BaseViewModel {
    public BaseViewModel SalesManListViewModel { get; }
    public BaseViewModel SellersViewModel { get; }

    public BaseViewModel ProductsViewModel { get; }

    public BaseViewModel SalesViewModel { get; }

    public BaseViewModel StatisticsViewModel { get; }

    public MainWindowViewModel(
        SelectedSalesManStore selectedSalesManStore, 
        SalesMenStore salesMenStore, 
        ProductsStore productsStore,
        ProductLinesStore productLinesStore
    ) {
        // Initialize ViewModels
        SalesManListViewModel = new SalesManListViewModel(selectedSalesManStore, salesMenStore);
        SellersViewModel = new SellersViewModel(selectedSalesManStore, salesMenStore);
        ProductsViewModel = new ProductsViewModel(productsStore);
        SalesViewModel = new SalesViewModel(selectedSalesManStore, productLinesStore, productsStore);
        StatisticsViewModel = new StatisticsViewModel(selectedSalesManStore, productLinesStore);

        // Load all sales men
        new LoadSalesMenCommand(salesMenStore).Execute(null);

        // Load all products
        new LoadProductsCommand(productsStore).Execute(null);   
    }
}