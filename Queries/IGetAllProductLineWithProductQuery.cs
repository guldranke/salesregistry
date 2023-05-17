using Sales.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sales.Queries;

public interface IGetAllProductLineWithProductQuery {
    Task<IEnumerable<ProductLineWithProduct>> Execute(SalesMan salesMan, DateTime startDate, DateTime endDate);
}
