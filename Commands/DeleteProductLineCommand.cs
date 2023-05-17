using Sales.Stores;
using Sales.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class DeleteProductLineCommand executes <see cref="ProductLinesStore.Delete"/> and handles any exceptions
/// </summary>
public class DeleteProductLineCommand : BaseCommandAsync {
    private readonly ProductLinesStore productLinesStore;
    private readonly SalesViewModel salesViewModel;

    public DeleteProductLineCommand(ProductLinesStore productLinesStore, SalesViewModel salesViewModel) {
        this.productLinesStore = productLinesStore;
        this.salesViewModel = salesViewModel;
    }

    public override async Task ExecuteAsync(object? parameter) {
        bool continueCommand =
          MessageBox.Show(
              "Er du sikker på du vil slette salget?",
              "Sletning af salg",
              MessageBoxButton.YesNo) == MessageBoxResult.Yes;

        if (!continueCommand || this.salesViewModel.SelectedProductLine == null) {
            return;
        }

        try {
            await this.productLinesStore.Delete(this.salesViewModel.SelectedProductLine);
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved sletning af salg\n{e.Message}");
        }
    }
}
