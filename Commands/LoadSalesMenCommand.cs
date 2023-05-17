using Sales.Stores;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class LoadSalesMenCommand executes <see cref="SalesMenStore.Load"/> and handles any exceptions
/// </summary>
public class LoadSalesMenCommand : BaseCommandAsync {
    private readonly SalesMenStore salesMenStore;

    public LoadSalesMenCommand(SalesMenStore salesMenStore) {
        this.salesMenStore = salesMenStore;
    }

    public override async Task ExecuteAsync(object? parameter) {
        try {
            await this.salesMenStore.Load();
        } catch (Exception e) {
            MessageBox.Show(e.Message);
        }
    }
}
