using System.Linq;
using System.Threading.Tasks;
using Data.Helper;
using Microsoft.AspNetCore.Http;

namespace Data.Repositories.Interfaces
{
    public interface IRepositoryExtension<T> where T : class
    {
        IQueryable<T> Sort(IQueryCollection query, IQueryable<T> results);
        IQueryable<T> Search(IQueryCollection query, IQueryable<T> results);
        Task<IPagedResult<T>> GetPage(IQueryCollection query, IQueryable<T> results);
    }
}
