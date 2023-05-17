using Sales.Models;
using System;
using System.Linq;

namespace Sales.Stores;

/// <summary>
/// Class SelectedSalesManStore is the single source of the selected sales man for the application
/// </summary>
public class SelectedSalesManStore {
    private SalesMan? selectedSalesMan;
    public SalesMan? SelectedSalesMan {
        get => selectedSalesMan; set {
            selectedSalesMan = value;
            SelectedSalesManChanged?.Invoke();
        }
    }

    public event Action? SelectedSalesManChanged;

    private readonly SalesMenStore salesMenStore;

    public SelectedSalesManStore(SalesMenStore salesMenStore) {
        this.salesMenStore = salesMenStore;

        // Listen to sales men store actions
        this.salesMenStore.SalesMenLoaded += SalesMenStore_SalesMenLoaded;
        this.salesMenStore.SalesMenDeleted += SalesMenStore_SalesMenDeleted;
        this.salesMenStore.SalesMenUpdated += SalesMenStore_SalesMenUpdated;
        this.salesMenStore.SalesMenCreated += SalesMenStore_SalesMenCreated;
    }

    private void SalesMenStore_SalesMenDeleted(SalesMan salesMan) {
        SelectedSalesMan = this.salesMenStore.SalesMen.First();
    }

    private void SalesMenStore_SalesMenLoaded() {
        SelectedSalesMan = this.salesMenStore.SalesMen.First();
    }

    private void SalesMenStore_SalesMenUpdated(SalesMan salesMan) {
        SelectedSalesMan = salesMan;
    }

    private void SalesMenStore_SalesMenCreated(SalesMan salesMan) {
        SelectedSalesMan = salesMan;
    }
}
