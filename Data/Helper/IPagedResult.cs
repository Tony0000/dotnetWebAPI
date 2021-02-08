using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Helper
{
    public interface IPagedResult<T> where T : class
    {
        public Metadata Metadata { get; set; }
        public Task<IPagedResult<T>> ToPagedResult(IQueryable<T> source, int pageNumber, int pageSize);
    }
}
