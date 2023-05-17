using Sales.Models;
using Sales.Stores;
using System;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class NewSalesManCommand executes <see cref="SalesMenStore.AddTemporary"/> and handles any exceptions
/// </summary>
public class NewSalesManCommand : BaseCommand {
    private readonly SalesMenStore salesMenStore;

    public NewSalesManCommand(SalesMenStore salesMenStore) {
        this.salesMenStore = salesMenStore;
    }

    public override void Execute(object? parameter) {
        try {
            this.salesMenStore.AddTemporary(new SalesMan(-1, "", "", EmployeeStatus.Active));
        } catch (Exception e) {
            MessageBox.Show($"Fejl ved opretning af midlertidig sælger\n{e.Message}");
        }
    }
}
