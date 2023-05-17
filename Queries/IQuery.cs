using System.Threading.Tasks;

namespace Sales.Queries;

public interface IQuery<T> {
    Task Execute(T item);
}
