namespace Sales.Models;


/// <summary>
/// Class Product models a Product which can be sold by a <see cref="SalesMan"/> in a <see cref="ProductLine"/>
/// </summary>
public class Product {
    // ProductId can't be readonly as it can be changed from temporary to real seller
    public int ProductId { get; set; }
    public string ProductName { get; } = string.Empty;
    public double Price { get; }

    public bool IsTemporary => ProductId == -1;

    public Product(int productId, string productName, double price) {
        ProductId = productId;
        ProductName = productName;
        Price = price;
    }

    public override string ToString() {
        return $"{ProductId} - {ProductName}";
    }
}

