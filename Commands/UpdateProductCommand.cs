using Sales.Models;
using Sales.Stores;
using Sales.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class UpdateProductCommand executes <see cref="ProductsStore.Update"/> and handles any exceptions
/// </summary>
public class UpdateProductCommand : BaseCommandAsync {
    private readonly ProductsStore productsStore;
    private readonly ProductsViewModel productsViewModel;

    public UpdateProductCommand(ProductsStore productsStore, ProductsViewModel productsViewModel) {
        this.productsStore = productsStore;
        this.productsViewModel = productsViewModel;
    }

    public override async Task ExecuteAsync(object? parameter) {
        ProductsFormViewModel form = this.productsViewModel.ProductsFormViewModel!;

        if (!form.CanSubmit) {
            MessageBox.Show("Navn/Pris kan ikke være tomt!");
            return;
        }

        int id = form.ProductId;
        string name = form.ProductName;
        double price = form.Price;

        Product? product = this.productsStore.Products.Find((p) => p.ProductName == name);

        if (product != null && id != product.ProductId) {
            bool continueCommand =
                MessageBox.Show(
                    "Produkt med navn findes allerede, fortsæt opdatering?",
                    "Opdatering af produkt",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes;

            if (!continueCommand) {
                return;
            }
        }

        Product updatedProduct = new(id, name, price);

        try {
            await this.productsStore.Update(updatedProduct);
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved opdatering af produkt\n{e.Message}");
        }
    }
}
