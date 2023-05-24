using Sales.Models;
using Sales.Stores;
using Sales.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class CreateProductLineCommand executes <see cref="Queries.CreateProductLineQuery"/> and handles any exceptions
/// </summary>
public class CreateProductLineCommand : BaseCommandAsync {
    private readonly ProductLinesStore productsLinesStore;
    private readonly SalesViewModel salesViewModel;
    private readonly SelectedSalesManStore selectedSalesManStore;

    public CreateProductLineCommand(ProductLinesStore productsLinesStore, SalesViewModel salesViewModel, SelectedSalesManStore selectedSalesManStore) {
        this.productsLinesStore = productsLinesStore;
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
            0,
            form.ProductId,
            this.selectedSalesManStore.SelectedSalesMan!.SalesManId,
            form.SalesDate,
            form.Price,
            form.Amount
        );

        try {
            await this.productsLinesStore.Add(productLine);
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved oprettelse af nyt salg\n{e.Message}");
        }
    }
}
