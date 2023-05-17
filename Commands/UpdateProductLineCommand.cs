using Sales.Models;
using Sales.Stores;
using Sales.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class UpdateProductLineCommand executes <see cref="ProductLinesStore.Update"/> and handles any exceptions
/// </summary>
public class UpdateProductLineCommand : BaseCommandAsync {
    private readonly ProductLinesStore productLinesStore;
    private readonly SalesViewModel salesViewModel;
    private readonly SelectedSalesManStore selectedSalesManStore;

    public UpdateProductLineCommand(ProductLinesStore productLinesStore, SalesViewModel salesViewModel, SelectedSalesManStore selectedSalesManStore) {
        this.productLinesStore = productLinesStore;
        this.salesViewModel = salesViewModel;
        this.selectedSalesManStore = selectedSalesManStore;
    }

    public override async Task ExecuteAsync(object? parameter) {
        SalesFormViewModel form = this.salesViewModel.SalesFormViewModel!;

        if (!form.CanSubmit) {
            MessageBox.Show("Pris/Antal/Produkt kan ikke være tomt!");
            return;
        }

        ProductLine productLine = new(
            form.ProdLineId,
            form.ProductId,
            this.selectedSalesManStore.SelectedSalesMan!.SalesManId,
            form.SalesDate,
            form.Price,
            form.Amount
        );

        try {
            await this.productLinesStore.Update(productLine);
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved opdatering af salg\n{e.Message}");
        }
    }
}
