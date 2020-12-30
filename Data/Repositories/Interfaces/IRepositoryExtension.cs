using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Data.Repositories.Interfaces
{
    public interface IRepositoryExtension<T>
    {
        IQueryable<T> Sort(IQueryCollection query, IQueryable<T> results);
        IQueryable<T> Search(IQueryCollection query, IQueryable<T> results);
        IQueryable<T> GetPage(IQueryCollection query, IQueryable<T> results);
    }
}
