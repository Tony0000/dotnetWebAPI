using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Repositories
{
    public interface IRepositoryExtension<T> where T : class
    {
        IQueryable<T> Sort(IQueryCollection query, IQueryable<T> results);
        IQueryable<T> Search(IQueryCollection query, IQueryable<T> results);
        Task<IPagedResult<T>> GetPage(IQueryCollection query, IQueryable<T> results);
    }
}
