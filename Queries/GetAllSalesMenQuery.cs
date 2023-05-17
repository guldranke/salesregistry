using Microsoft.EntityFrameworkCore;
using Sales.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Queries;

/// <summary>
/// Class GetAllSalesMenQuery gets all <see cref="SalesMan"/>'s from the Database
/// </summary>
public class GetAllSalesMenQuery : IGetAllQuery<SalesMan> {
    private readonly SalesDbContextFactory contextFactory;

    public GetAllSalesMenQuery(SalesDbContextFactory contextFactory) {
        this.contextFactory = contextFactory;
    }

    /// <returns>
    /// IEnumerable of all active sales men in the Database
    /// </returns>
    public async Task<IEnumerable<SalesMan>> Execute() {
        using SalesDbContext context = this.contextFactory.Create();

        var salesMen = await context.SalesMen.Where(s => s.EmployeeStat == 1).ToListAsync();

        return salesMen.Select((s) => new SalesMan(s.SalesManId, s.Firstname, s.Lastname, (EmployeeStatus)s.EmployeeStat));
    }
}
