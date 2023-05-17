using System;

namespace Sales.Models;

/// <summary>
/// Class ProductLineWithProduct models a ProductLine with the product name
/// </summary>
public class ProductLineWithProduct : ProductLine {
    public string ProductName { get; } = string.Empty;
    public ProductLineWithProduct(
        int prodLineId,
        int productId,
        int salesManId,
        DateTime salesDate,
        double price,
        int amount,
        string productName
    ) : base(prodLineId, productId, salesManId, salesDate, price, amount) {
        ProductName = productName;
    }
}
