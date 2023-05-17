namespace Sales.Models;
public enum EmployeeStatus {
    Inactive = 0,
    Active = 1
}

/// <summary>
/// Class SalesMan models a SalesMan
/// </summary>
public class SalesMan {
    // SalesManId can't be readonly as it can be changed from temporary to real seller
    public int SalesManId { get; set;  }
    public string Firstname { get; } = string.Empty;
    public string Lastname { get; } = string.Empty;
    public string Fullname => $"{Firstname} {Lastname}";
    public EmployeeStatus EmployeeStat { get; }
    public bool IsTemporary => SalesManId == -1;

    public SalesMan(int salesManId, string firstname, string lastname, EmployeeStatus employeeStat) {
        SalesManId = salesManId;
        Firstname = firstname;
        Lastname = lastname;
        EmployeeStat = employeeStat;
    }
}

