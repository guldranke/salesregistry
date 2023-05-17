using Sales.DTOs;
using Sales.Models;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class DeleteSalesManQuery deletes a <see cref="SalesMan"/> from the database
/// </summary>
/// <remarks>
/// The <see cref="SalesMan"/> is not actually deleted! Just set to inactive
/// </remarks>
public class DeleteSalesManQuery : IQuery<SalesMan> {
    private readonly SalesDbContextFactory contextFactory;

    public DeleteSalesManQuery(SalesDbContextFactory contextFactory) {
        this.contextFactory = contextFactory;
    }

    public async Task Execute(SalesMan salesMan) {
        using SalesDbContext context = this.contextFactory.Create();

        SalesManDto salesManDto = new() {
            SalesManId = salesMan.SalesManId,
            Firstname = salesMan.Firstname,
            Lastname = salesMan.Lastname,
            EmployeeStat = 0
        };

        context.SalesMen.Update(salesManDto);
        await context.SaveChangesAsync();
    }
}
