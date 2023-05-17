using Sales.Commands;
using Sales.Models;
using Sales.Stores;
using System.Windows.Input;

namespace Sales.ViewModels;

/// <summary>
/// Class SellersViewModel models the <see cref="Views.Sellers.Sellers"/> view
/// </summary>
public class SellersViewModel : BaseViewModel {
    private readonly SelectedSalesManStore selectedSalesManStore;

    public SalesMan? SelectedSalesMan => selectedSalesManStore.SelectedSalesMan;

    public SellersFormViewModel? SellersFormViewModel { get; set; }

    public ICommand? NewCommand { get; }
    public ICommand? CreateCommand { get; }
    public ICommand? UpdateCommand { get; }
    public ICommand? DeleteCommand { get; }

    public SellersViewModel(SelectedSalesManStore selectedSalesManStore, SalesMenStore salesMenStore) {
        this.selectedSalesManStore = selectedSalesManStore;

        // Initialize commands
        NewCommand = new NewSalesManCommand(salesMenStore);
        CreateCommand = new CreateSalesManCommand(salesMenStore, this);
        UpdateCommand = new UpdateSalesManCommand(salesMenStore, this);
        DeleteCommand = new DeleteSalesManCommand(salesMenStore, selectedSalesManStore);

        // Listen to selected sales man store actions
        this.selectedSalesManStore.SelectedSalesManChanged += SelectedSalesManStore_SelectedSalesManChanged;
    }

    protected override void Dispose() {
        this.selectedSalesManStore.SelectedSalesManChanged -= SelectedSalesManStore_SelectedSalesManChanged;
        base.Dispose();
    }

    private void SelectedSalesManStore_SelectedSalesManChanged() {
        SellersFormViewModel = new(this.SelectedSalesMan!);
        OnPropertyChanged(nameof(SellersFormViewModel));
        OnPropertyChanged(nameof(SelectedSalesMan));
    }
}
