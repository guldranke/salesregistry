using Sales.Models;
using Sales.Stores;
using Sales.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class UpdateSalesManCommand executes <see cref="SalesMenStore.Update"/> and handles any exceptions
/// </summary>
public class UpdateSalesManCommand : BaseCommandAsync {
    private readonly SalesMenStore salesMenStore;
    private readonly SellersViewModel sellersViewModel;

    public UpdateSalesManCommand(SalesMenStore salesMenStore, SellersViewModel SellersViewModel) {
        this.salesMenStore = salesMenStore;
        this.sellersViewModel = SellersViewModel;
    }

    public override async Task ExecuteAsync(object? parameter) {
        SellersFormViewModel form = this.sellersViewModel.SellersFormViewModel!;

        if (!form.CanSubmit) {
            MessageBox.Show("Fornavn/Efternavn kan ikke være tomt!");
            return;
        }

        int id = form.SalesManId;
        string firstname = form.Firstname;
        string lastname = form.Lastname;

        SalesMan? seller = this.salesMenStore.SalesMen.Find((s) => s.Firstname == firstname && s.Lastname == lastname);

        if (seller != null && id != seller.SalesManId) {
            bool continueCommand =
                MessageBox.Show(
                    "Sælger med fornavn og efternavn findes allerede, fortsæt opdatering?",
                    "Opdatering af sælger",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes;

            if (!continueCommand) {
                return;
            }
        }

        SalesMan salesMan = new(
            id,
            firstname,
            lastname,
            EmployeeStatus.Active
        );

        try {
            await this.salesMenStore.Update(salesMan);
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved opdatering af sælger\n{e.Message}");
        }
    }
}
