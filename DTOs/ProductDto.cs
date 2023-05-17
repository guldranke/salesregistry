using Sales.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.DTOs;

/// <summary>
/// Class ProductDto models a data transfer object for <see cref="Product"/>
/// </summary>
public class ProductDto {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public double Price { get; set; }
}
