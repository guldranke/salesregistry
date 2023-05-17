using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sales.Queries;

public interface IGetAllQuery<T> {
    Task<IEnumerable<T>> Execute();
}
