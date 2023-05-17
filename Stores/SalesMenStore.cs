using Sales.Models;
using Sales.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Stores;

/// <summary>
/// Class SalesMenStore is the single source of all sales men for the application
/// </summary>
public class SalesMenStore {
    private readonly IGetAllQuery<SalesMan> getAllSalesMenQuery;
    private readonly IQuery<SalesMan> createSalesManQuery;
    private readonly IQuery<SalesMan> updateSalesManQuery;
    private readonly IQuery<SalesMan> deleteSalesManQuery;

    private readonly List<SalesMan> salesMen;
    public List<SalesMan> SalesMen => salesMen;

    public event Action? SalesMenLoaded;
    public event Action<SalesMan>? SalesMenCreated;
    public event Action<SalesMan>? SalesMenUpdated;
    public event Action<SalesMan>? SalesMenDeleted;

    public SalesMenStore(
        IGetAllQuery<SalesMan> getAllSalesMenQuery,
        IQuery<SalesMan> createSalesManQuery,
        IQuery<SalesMan> updateSalesManQuery,
        IQuery<SalesMan> deleteSalesManQuery) {

        this.getAllSalesMenQuery = getAllSalesMenQuery;
        this.createSalesManQuery = createSalesManQuery;
        this.updateSalesManQuery = updateSalesManQuery;
        this.deleteSalesManQuery = deleteSalesManQuery;

        this.salesMen = new();
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.GetAllSalesMenQuery"/>
    /// </summary>
    public async Task Load() {
        IEnumerable<SalesMan> loadedSalesMen = await this.getAllSalesMenQuery.Execute();

        this.salesMen.Clear();

        if (!loadedSalesMen.Any()) {
            this.salesMen.Add(new SalesMan(-1, "", "", EmployeeStatus.Active));
        } else {
            this.salesMen.AddRange(loadedSalesMen);
        }

        SalesMenLoaded?.Invoke();
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.CreateSalesManQuery"/>
    /// </summary>
    public async Task Add(SalesMan salesMan) {
        await this.createSalesManQuery.Execute(salesMan);

        int index = this.salesMen.FindIndex((s) => s.SalesManId == -1);

        if (index == -1) {
            this.salesMen.Add(salesMan);
        } else {
            this.salesMen[index] = salesMan;
        }

        SalesMenCreated?.Invoke(salesMan);
    }

    /// <summary>
    /// Synchronously adds a temporary sales man
    /// </summary>
    public void AddTemporary(SalesMan salesMan) {
        if (this.salesMen.Find((s) => s.SalesManId == -1) == null) {
            this.salesMen.Add(salesMan);
            SalesMenCreated?.Invoke(salesMan);
        }
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.UpdateSalesManQuery"/>
    /// </summary>
    public async Task Update(SalesMan salesMan) {
        await this.updateSalesManQuery.Execute(salesMan);

        int index = this.salesMen.FindIndex(s => s.SalesManId == salesMan.SalesManId);

        if (index == -1) {
            this.salesMen.Add(salesMan);
        } else {
            this.salesMen[index] = salesMan;
        }

        SalesMenUpdated?.Invoke(salesMan);
    }

    /// <summary>
    /// Asynchronously executes the <see cref="Queries.DeleteSalesManQuery"/>
    /// </summary>
    public async Task Delete(SalesMan salesMan) {
        if (!salesMan.IsTemporary) {
            await this.deleteSalesManQuery.Execute(salesMan);
        }

        this.salesMen.RemoveAll((s) => s.SalesManId == salesMan.SalesManId);

        if (this.SalesMen.Count == 0) {
            AddTemporary(new SalesMan(0, "", "", EmployeeStatus.Active));
        }

        SalesMenDeleted?.Invoke(salesMan);
    }
}
