using Sales.Models;
using System;
using System.Collections.Generic;

namespace Sales.ViewModels;

/// <summary>
/// Class SalesFormViewModel models the form for <see cref="Views.Sales.Sales"/>
/// </summary>
public class SalesFormViewModel : BaseViewModel {
    private int prodLineId;
    public int ProdLineId { 
        get => prodLineId; set {
            prodLineId = value;
            OnPropertyChanged();
        }
    }

    private int productId;
    public int ProductId {
        get => productId; set {
            productId = value;
            OnPropertyChanged();
        }
    }

    private DateTime salesDate;
    public DateTime SalesDate {
        get => salesDate; set {
            salesDate = value;
            OnPropertyChanged();
        }
    }

    private Product? selectedProduct;
    public Product? SelectedProduct {
        get => selectedProduct; set {
            selectedProduct = value;
            if(value != null) {
                Price = value!.Price;
                ProductId = value!.ProductId;
            }
            OnPropertyChanged();
            OnPropertyChanged(nameof(ProductDisplay));
        }
    }

    public string ProductDisplay => this.selectedProduct?.ToString() ?? "";

    private double price;
    public double Price {
        get => price; set {
            price = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TotalPrice));
        }
    }

    private int amount;
    public int Amount {
        get => amount; set {
            amount = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TotalPrice));
        }
    }

    public double TotalPrice => amount * price;

    public bool CanSubmit => price != 0.00 && amount != 0 && productId != -1;

    public SalesFormViewModel(ProductLine productLine, List<Product> products) { 
        ProdLineId = productLine.ProdLineId;
        ProductId = productLine.ProductId;
        Price = productLine.Price;
        Amount = productLine.Amount;
        SalesDate = productLine.SalesDate;

        SelectedProduct = products.Find((p) => p.ProductId == ProductId);
    }
}
