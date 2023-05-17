using Sales.Models;
using Sales.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sales.ViewModels;

/// <summary>
/// Class SalesManListViewModel models the <see cref="Views.SalesManListView.SalesManListView"/> view
/// </summary>
public class SalesManListViewModel : BaseViewModel {
    private readonly ObservableCollection<SalesManViewModel> salesMen;
    public IEnumerable<SalesManViewModel> SalesMen => salesMen;

    public SalesManViewModel? SelectedSalesManViewModel {
        get {
            return this.salesMen.FirstOrDefault((s) => s.SalesManId == this.selectedSalesManStore.SelectedSalesMan!.SalesManId);
        }
        set {
            this.selectedSalesManStore.SelectedSalesMan = value?.SalesMan;
        }
    }

    private readonly SalesMenStore salesMenStore;
    private readonly SelectedSalesManStore selectedSalesManStore;

    public SalesManListViewModel(SelectedSalesManStore selectedSalesManStore, SalesMenStore salesMenStore) {
        this.selectedSalesManStore = selectedSalesManStore;
        this.salesMenStore = salesMenStore;

        this.salesMen = new();

        // Listen to sales men store actions
        this.salesMenStore.SalesMenLoaded += SalesMenStore_salesMenLoaded;
        this.salesMenStore.SalesMenCreated += SalesMenStore_salesMenCreated;
        this.salesMenStore.SalesMenUpdated += SalesMenStore_salesMenUpdated;
        this.salesMenStore.SalesMenDeleted += SalesMenStore_salesMenDeleted;

        // Listen to selected sales man store actions
        this.selectedSalesManStore.SelectedSalesManChanged += SelectedSalesManStore_SelectedSalesManChanged;
    }

    protected override void Dispose() {
        this.salesMenStore.SalesMenLoaded -= SalesMenStore_salesMenLoaded;
        this.salesMenStore.SalesMenCreated -= SalesMenStore_salesMenCreated;
        this.salesMenStore.SalesMenUpdated -= SalesMenStore_salesMenUpdated;
        this.salesMenStore.SalesMenDeleted -= SalesMenStore_salesMenDeleted;

        this.selectedSalesManStore.SelectedSalesManChanged -= SelectedSalesManStore_SelectedSalesManChanged;

        base.Dispose();
    }

    private void SelectedSalesManStore_SelectedSalesManChanged() {
        OnPropertyChanged(nameof(SelectedSalesManViewModel));
    }

    private void SalesMenStore_salesMenLoaded() {
        this.salesMen.Clear();

        foreach (SalesMan salesMan in this.salesMenStore.SalesMen) {
            this.salesMen.Add(new SalesManViewModel(salesMan));
        }

        OnPropertyChanged(nameof(SelectedSalesManViewModel));
    }

    private void SalesMenStore_salesMenDeleted(SalesMan salesMan) {
        SalesManViewModel? deletedSalesMan = this.salesMen.FirstOrDefault((s) => s.SalesManId == salesMan.SalesManId);

        if (deletedSalesMan != null) {
            this.salesMen.Remove(deletedSalesMan);
        }

        OnPropertyChanged(nameof(SelectedSalesManViewModel));
    }

    private void SalesMenStore_salesMenUpdated(SalesMan salesMan) {
        SalesManViewModel? salesManViewModel =
            this.salesMen.FirstOrDefault((s) => s.SalesManId == salesMan.SalesManId);

        salesManViewModel?.Update(salesMan);

        OnPropertyChanged(nameof(SelectedSalesManViewModel));
    }

    private void SalesMenStore_salesMenCreated(SalesMan salesMan) {
        SalesManViewModel? salesManViewModel = this.salesMen.FirstOrDefault((s) => s.SalesManId == -1);

        if (salesManViewModel != null) {
            // Remove the temporary seller upon creating a new real seller  
            int index = this.salesMen.IndexOf(salesManViewModel);
            this.salesMen[index].Update(salesMan);
        } else {
            this.salesMen.Add(new SalesManViewModel(salesMan));
        }

        OnPropertyChanged(nameof(SelectedSalesManViewModel));
    }
}
