using Sales.Models;
using Sales.Stores;
using Sales.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class CreateProductCommand executes <see cref="ProductsStore.Add"/> and handles any exceptions
/// </summary>
public class CreateProductCommand : BaseCommandAsync {
    private readonly ProductsStore productsStore;
    private readonly ProductsViewModel productsViewModel;

    public CreateProductCommand(ProductsStore productsStore, ProductsViewModel ProductsViewModel) {
        this.productsStore = productsStore;
        this.productsViewModel = ProductsViewModel;
    }

    public override async Task ExecuteAsync(object? parameter) {
        ProductsFormViewModel form = this.productsViewModel.ProductsFormViewModel!;

        if (!form.CanSubmit) {
            MessageBox.Show("Navn/Pris kan ikke være tomt!");
            return;
        }

        string productName = form.ProductName;
        double price = form.Price;

        if (this.productsStore.Products.Any((p) => p.ProductName == productName)) {
            bool continueCommand =
                MessageBox.Show(
                    "Produkt med navn findes allerede, fortsæt opretning?",
                    "Opretning af produkt",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes;

            if (!continueCommand) {
                return;
            }
        }

        Product product = new(0, productName, price);

        try {
            await this.productsStore.Add(product);
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved oprettelse af nyt produkt\n{e.Message}");
        }
    }
}
