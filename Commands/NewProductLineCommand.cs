using Sales.Models;
using Sales.Stores;
using System;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class NewProductLineCommand executes <see cref="ProductLinesStore.AddTemporary(ProductLine)"/> and handles any exceptions
/// </summary>
public class NewProductLineCommand : BaseCommand {
    private readonly ProductLinesStore productLinesStore;

    public NewProductLineCommand(ProductLinesStore productLinesStore) {
        this.productLinesStore = productLinesStore;
    }

    public override void Execute(object? parameter) {
        try {
            this.productLinesStore.AddTemporary(new ProductLine(-1, 0, 0, DateTime.Now, 0.00, 0));
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved opretning af midlertidig salg\n{e.Message}");
        }
    }
}
