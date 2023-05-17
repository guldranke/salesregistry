using Sales.Models;
using System;

namespace Sales.ViewModels;

/// <summary>
/// Class ProductLineViewModel is the ViewModel for a <see cref="Models.ProductLine"/>
/// </summary>
public class ProductLineViewModel : BaseViewModel {
    public ProductLine ProductLine { get; private set; }

    public int ProdLineId => ProductLine.ProdLineId;
    public int ProductId => ProductLine.ProductId;
    public int SalesManId => ProductLine.SalesManId;
    public DateTime SalesDate => ProductLine.SalesDate;
    public double Price => ProductLine.Price;
    public int Amount => ProductLine.Amount;
    public double TotalPrice => Price * Amount;

    public ProductLineViewModel(ProductLine productLine) {
        ProductLine = productLine;
    }

    public void Update(ProductLine productLine) {
        ProductLine = productLine;

        OnPropertyChanged(nameof(ProductLine));
    }
}
