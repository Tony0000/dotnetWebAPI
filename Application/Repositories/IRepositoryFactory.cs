using System.Threading;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IRepositoryFactory
    {
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
