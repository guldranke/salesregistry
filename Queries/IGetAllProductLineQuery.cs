using Sales.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sales.Queries;

public interface IGetAllProductLineQuery {
    Task<IEnumerable<ProductLine>> Execute(SalesMan salesMan);
}
