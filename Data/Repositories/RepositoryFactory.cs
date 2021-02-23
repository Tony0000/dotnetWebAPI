using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Persistence.Repositories.Interfaces;

namespace Persistence.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IWebApiDbContext _repoContext;
        private IUserRepository _userRepository;

        public RepositoryFactory(IWebApiDbContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public IUserRepository Users
            => _userRepository ??= new UserRepository(_repoContext);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _repoContext.SaveChangesAsync(cancellationToken);
        }
    }
}
