using Sales.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.DTOs;

/// <summary>
/// Class ProductLineDto models a data transfer object for <see cref="ProductLine"/>
/// </summary>
public class ProductLineDto {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProdLineId { get; set; }
    public int ProductId { get; set; }
    public int SalesManId { get; set; }
    public DateTime SalesDate { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
    public double TotalPrice { get; set; }
}
