using Sales.Stores;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class DeleteSalesManCommand executes <see cref="SalesMenStore.Delete"/> and handles any exceptions
/// </summary>
public class DeleteSalesManCommand : BaseCommandAsync {
    private readonly SalesMenStore salesMenStore;
    private readonly SelectedSalesManStore selectedSalesManStore;

    public DeleteSalesManCommand(SalesMenStore salesMenStore, SelectedSalesManStore selectedSalesManStore) {
        this.salesMenStore = salesMenStore;
        this.selectedSalesManStore = selectedSalesManStore;
    }

    public override async Task ExecuteAsync(object? parameter) {
        bool continueCommand =
          MessageBox.Show(
              "Er du sikker på du vil slette sælger?",
              "Sletning af sælger",
              MessageBoxButton.YesNo) == MessageBoxResult.Yes;

        if (!continueCommand) {
            return;
        }

        try {
            await this.salesMenStore.Delete(this.selectedSalesManStore.SelectedSalesMan!);
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved slettelse af sælger\n{e.Message}");
        }
    }
}
