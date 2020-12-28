using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        IUserRepository Users { get; }
        Task SaveChangesAsync();
    }
}
