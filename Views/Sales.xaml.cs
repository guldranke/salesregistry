using Sales.Models;
using Sales.ViewModels;
using System.Linq;
using System.Windows.Controls;


namespace Sales.Views;

/// <summary>
/// Interaction logic for Sales.xaml
/// </summary>
public partial class Sales : UserControl {
    public Sales() {
        InitializeComponent();
    }

    /// <summary>
    /// Search for a product line based on a mode
    /// </summary>
    private void Search() {
        SalesViewModel context = (SalesViewModel)DataContext;
        string query = context.SearchQuery;

        switch (context.SearchMode) {
            case SearchType.ProductName: {
                    var product = context.Products.Where((p) => p.ProductName.StartsWith(query));

                    if (!product.Any()) break;

                    var productLine = context.ProductLines.Where((p) => p.ProductId == product.First().ProductId);

                    if (!productLine.Any()) break;

                    context.SelectedProductLine = productLine.First();
                    break;
                }
            case SearchType.SalesDate: {
                    var productLine = context.ProductLines.Where((p) => p.SalesDate.ToString().StartsWith(query));

                    if (!productLine.Any()) break;

                    context.SelectedProductLine = productLine.First();
                    break;
                }
            default:
                System.Diagnostics.Trace.WriteLine("How did you get here?");
                break;
        }
    }

    /// <summary>
    /// Go to the first element in the navigator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NavigatorFirst(object sender, System.Windows.RoutedEventArgs e) {
        SalesViewModel context = (SalesViewModel)DataContext;
        context.SelectedProductLine = context.ProductLines.First();
    }

    /// <summary>
    /// Go to the last element in the navigator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NavigatorLast(object sender, System.Windows.RoutedEventArgs e) {
        SalesViewModel context = (SalesViewModel)DataContext;
        context.SelectedProductLine = context.ProductLines.Last();
    }

    /// <summary>
    /// Go to the previous element in the navigator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NavigatorLeft(object sender, System.Windows.RoutedEventArgs e) {
        SalesViewModel context = (SalesViewModel)DataContext;

        int index = context.ProductLines.FindIndex((p) => p.ProdLineId == context.SelectedProductLine!.ProdLineId);

        if (index != 0) {
            context.SelectedProductLine = context.ProductLines[index - 1];
        }
    }

    /// <summary>
    /// Go to the next element in the navigator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NavigatorRight(object sender, System.Windows.RoutedEventArgs e) {
        SalesViewModel context = (SalesViewModel)DataContext;

        int index = context.ProductLines.FindIndex((p) => p.ProdLineId == context.SelectedProductLine!.ProdLineId);

        if (index != context.ProductLines.Count - 1) {
            context.SelectedProductLine = context.ProductLines[index + 1];
        }
    }

    /// <summary>
    /// Set <see cref="SalesViewModel.SearchMode"/> to <see cref="SearchType.ProductName"/> and call <see cref="Search"/>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SearchModeName(object sender, System.Windows.RoutedEventArgs e) {
        SalesViewModel context = (SalesViewModel)DataContext;

        context.SearchMode = SearchType.ProductName;
        Search();
    }

    /// <summary>
    /// Set <see cref="SalesViewModel.SearchMode"/> to <see cref="SearchType.SalesDate"/> and call <see cref="Search"/>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SearchModeDate(object sender, System.Windows.RoutedEventArgs e) {
        SalesViewModel context = (SalesViewModel)DataContext;

        context.SearchMode = SearchType.SalesDate;
        Search();
    }
}

