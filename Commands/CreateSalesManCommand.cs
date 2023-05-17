using Sales.Models;
using Sales.Stores;
using Sales.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class CreateSalesManCommand executes <see cref="SalesMenStore.Add"/> and handles any exceptions
/// </summary>
public class CreateSalesManCommand : BaseCommandAsync {
    private readonly SalesMenStore salesMenStore;
    private readonly SellersViewModel sellersViewModel;

    public CreateSalesManCommand(SalesMenStore salesMenStore, SellersViewModel SellersViewModel) {
        this.salesMenStore = salesMenStore;
        this.sellersViewModel = SellersViewModel;
    }

    public override async Task ExecuteAsync(object? parameter) {
        SellersFormViewModel form = this.sellersViewModel.SellersFormViewModel!;

        if (!form.CanSubmit) {
            MessageBox.Show("Fornavn/Efternavn kan ikke være tomt!");
            return;
        }

        string firstname = form.Firstname;
        string lastname = form.Lastname;

        if (this.salesMenStore.SalesMen.Any((s) => s.Firstname == firstname && s.Lastname == lastname)) {
            bool continueCommand = 
                MessageBox.Show(
                    "Sælger med fornavn og efternavn findes allerede, fortsæt opretning?", 
                    "Opretning af sælger", 
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes;

            if (!continueCommand) {
                return;
            }
        }

        SalesMan salesMan = new(
            0,
            firstname,
            lastname,
            EmployeeStatus.Active
        );

        try {
            await this.salesMenStore.Add(salesMan);
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved oprettelse af ny sælger\n{e.Message}");
        }
    }
}
