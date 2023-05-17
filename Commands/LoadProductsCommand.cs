using Sales.Stores;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Sales.Commands;

/// <summary>
/// Class LoadProductsCommand executes <see cref="ProductsStore.Load"/> and handles any exceptions
/// </summary>
public class LoadProductsCommand : BaseCommandAsync {
    private readonly ProductsStore productsStore;
    public LoadProductsCommand(ProductsStore productsStore) {
        this.productsStore = productsStore;
    }

    public override async Task ExecuteAsync(object? parameter) {
        try {
            await this.productsStore.Load();
        } catch (Exception e) {
            MessageBox.Show(e.Message);
        }
    }
}
