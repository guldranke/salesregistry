using Sales.Models;

namespace Sales.ViewModels;

/// <summary>
/// Class ProductViewModel is the ViewModel for a <see cref="Models.Product"/>
/// </summary>
public class ProductViewModel : BaseViewModel {
    public Product Product { get; private set; }

    public int ProductId => Product.ProductId;
    public string ProductName => Product.ProductName;
    public double Price => Product.Price;

    public ProductViewModel(Product product) {
        Product = product;
    }

    public void Update(Product product) {
        Product = product;

        OnPropertyChanged(nameof(Product));
    }
}
