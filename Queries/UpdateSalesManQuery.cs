using Sales.DTOs;
using Sales.Models;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class UpdateSalesManQuery updates a <see cref="SalesMan"/> which is saved in the Database
/// </summary>
public class UpdateSalesManQuery : IQuery<SalesMan> {
    private readonly SalesDbContextFactory contextFactory;

    public UpdateSalesManQuery(SalesDbContextFactory contextFactory) { 
        this.contextFactory = contextFactory;
    }

    public async Task Execute(SalesMan salesMan) {
        using SalesDbContext context = this.contextFactory.Create();

        SalesManDto salesManDto = new() {
            SalesManId = salesMan.SalesManId,
            Firstname = salesMan.Firstname,
            Lastname = salesMan.Lastname,
            EmployeeStat = (int)salesMan.EmployeeStat
        };

        context.SalesMen.Update(salesManDto);
        await context.SaveChangesAsync();
    }
}
