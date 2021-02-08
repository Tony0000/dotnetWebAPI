using System.Threading.Tasks;

namespace Persistence.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
