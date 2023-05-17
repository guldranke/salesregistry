using System;

namespace Sales.Models;

/// <summary>
/// Class ProductLine models a ProductLine
/// </summary>
public class ProductLine {
    // ProdLineId can't be readonly as it can be changed from temporary to real seller
    public int ProdLineId { get; set;  }
    public int ProductId { get; }
    public int SalesManId { get; }
    public DateTime SalesDate { get; }
    public double Price { get; }
    public int Amount { get; }
    public double TotalPrice => Price * Amount;
    public bool IsTemporary => ProdLineId == -1;

    public ProductLine(
        int prodLineId,
        int productId,
        int salesManId,
        DateTime salesDate,
        double price,
        int amount
    ) {
        ProdLineId = prodLineId;
        ProductId = productId;
        SalesManId = salesManId;
        SalesDate = salesDate;
        Price = price;
        Amount = amount;
    }
}
