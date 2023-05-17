using Sales.Models;
using Sales.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Sales.Views;

/// <summary>
/// Interaction logic for Products.xaml
/// </summary>
public partial class Products : UserControl {
    public Products() {
        InitializeComponent();
    }

    /// <summary>
    /// Search for a product via productName
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Search_TextChanged(object sender, TextChangedEventArgs e) {
        ProductsViewModel context = (ProductsViewModel)DataContext;
        string query = ((TextBox)sender).Text;

        IEnumerable<Product?> found = context.Products.Where((p) => p.ProductName.StartsWith(query));

        if (!found.Any()) return;

        context.SelectedProduct = found.First();
    }

    /// <summary>
    /// Go to the first element in the navigator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NavigatorFirst(object sender, System.Windows.RoutedEventArgs e) {
        ProductsViewModel context = (ProductsViewModel)DataContext;
        context.SelectedProduct = context.Products.First();
    }

    /// <summary>
    /// Go to the last element in the navigator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NavigatorLast(object sender, System.Windows.RoutedEventArgs e) {
        ProductsViewModel context = (ProductsViewModel)DataContext;
        context.SelectedProduct = context.Products.Last();
    }

    /// <summary>
    /// Go to the previous element in the navigator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NavigatorLeft(object sender, System.Windows.RoutedEventArgs e) {
        ProductsViewModel context = (ProductsViewModel)DataContext;

        int index = context.Products.FindIndex((p) => p.ProductId == context.SelectedProduct!.ProductId);

        if(index != 0) {
            context.SelectedProduct = context.Products[index - 1];
        } 
    }

    /// <summary>
    /// Go to the next element in the navigator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NavigatorRight(object sender, System.Windows.RoutedEventArgs e) {
        ProductsViewModel context = (ProductsViewModel)DataContext;

        int index = context.Products.FindIndex((p) => p.ProductId == context.SelectedProduct!.ProductId);

        if (index != context.Products.Count - 1) {
            context.SelectedProduct = context.Products[index + 1];
        }
    }
}
