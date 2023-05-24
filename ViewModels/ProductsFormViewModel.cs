using Sales.Models;

namespace Sales.ViewModels;

/// <summary>
/// Class ProductsFormViewModel models the form for <see cref="Views.Products.Products"/>
/// </summary>
public class ProductsFormViewModel : BaseViewModel {
    private bool isTemporary;
    public bool IsTemporary {
        get => isTemporary; set {
            isTemporary = value;
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

    private string productName = string.Empty;
    public string ProductName {
        get => productName; set {
            productName = value;
            OnPropertyChanged();
        }
    }

    private double price;
    public double Price {
        get => price; set {
            price = value;
            OnPropertyChanged(); 
        }
    }

    public bool CanSubmit => !string.IsNullOrEmpty(productName) && price != 0.00;

    public ProductsFormViewModel(Product product) {
        ProductId = product.ProductId;
        ProductName = product.ProductName;
        Price = product.Price;
        IsTemporary = product.IsTemporary;
    }
}
