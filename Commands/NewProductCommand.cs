using Sales.Models;
using Sales.Stores;
using System;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class NewProductCommand executes <see cref="ProductsStore.AddTemporary"/> and handles any exceptions
/// </summary>
public class NewProductCommand : BaseCommand {
    private readonly ProductsStore productsStore;

    public NewProductCommand(ProductsStore productsStore) {
        this.productsStore = productsStore;
    }

    public override void Execute(object? parameter) {
        try {
            this.productsStore.AddTemporary(new Product(-1, "", 0.00));
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved opretning af midlertidig produkt\n{e.Message}");
        }
    }
}
