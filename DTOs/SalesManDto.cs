using Sales.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.DTOs;

/// <summary>
/// Class SalesManDto models a data transfer object for <see cref="SalesMan"/>
/// </summary>
public class SalesManDto {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesManId { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public int EmployeeStat { get; set; }
}
