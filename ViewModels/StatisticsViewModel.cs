using Sales.Models;
using Sales.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sales.ViewModels;

/// <summary>
/// Class StatisticsViewModel models the <see cref="Views.Statistics.Statistics"/> view
/// </summary>
public class StatisticsViewModel : BaseViewModel {
    private readonly SelectedSalesManStore selectedSalesManStore;
    private readonly ProductLinesStore productLinesStore;

    private DateTime startDate = DateTime.Now - TimeSpan.FromDays(14);
    public DateTime StartDate {
        get => startDate; set {
            if(value < this.endDate) {
                startDate = value;
            } else {
                // If date is later than EndDate, it is set to EndDate as it's the latest date allowed
                startDate = this.EndDate;
            }
            OnPropertyChanged();
        }
    }

    private DateTime endDate = DateTime.Now;
    public DateTime EndDate {
        get => endDate; set {
            if (value >= this.startDate) {
                endDate = value;
            } else {
                // If date is earlier than StartDate, it is set to StartDate as it's the earliest date allowed
                endDate = this.StartDate;
            }
            OnPropertyChanged();
        }
    }

    private readonly ObservableCollection<ProductLineWithProduct> productLines;
    public IEnumerable<ProductLineWithProduct> ProductLines => productLines;

    public double TotalSalesPrice => this.productLines.Select((p) => p.TotalPrice).Sum();

    public ICommand? GetSalesCommand { get; }

    public StatisticsViewModel(SelectedSalesManStore selectedSalesManStore, ProductLinesStore productLinesStore) {
        this.selectedSalesManStore = selectedSalesManStore;
        this.productLinesStore = productLinesStore;

        this.productLines = new();

        this.selectedSalesManStore.SelectedSalesManChanged += SelectedSalesManStore_SelectedSalesManChanged;
    }

    protected override void Dispose() {
        this.selectedSalesManStore.SelectedSalesManChanged -= SelectedSalesManStore_SelectedSalesManChanged;
        base.Dispose();
    }

    /// <summary>
    /// On selected sales man change, reset the dates and get the sales
    /// </summary>
    private void SelectedSalesManStore_SelectedSalesManChanged() {
        ResetDates();
        GetGetSales();
    }

    /// <summary>
    /// Reset the <see cref="StartDate"/> and <see cref="EndDate"/>
    /// </summary>
    private void ResetDates() {
        StartDate = DateTime.Now - TimeSpan.FromDays(14);
        EndDate = DateTime.Now;
    }

    /// <summary>
    /// Clears the <see cref="productLines"/> and gets the product lines belonging 
    /// to a sales man which is then added to the <see cref="productLines"/>
    /// </summary>
    public async void GetGetSales() {
        if (this.selectedSalesManStore.SelectedSalesMan == null) return;

        this.productLines.Clear();

        IEnumerable<ProductLineWithProduct> productLines =
            await this.productLinesStore.GetProductLineWithProducts(this.selectedSalesManStore.SelectedSalesMan, this.startDate, this.endDate);

        foreach (ProductLineWithProduct productLine in productLines) {
            this.productLines.Add(productLine);
        }

        OnPropertyChanged(nameof(TotalSalesPrice));
    }
}
