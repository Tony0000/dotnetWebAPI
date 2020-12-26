﻿using Data.Repositories.Interfaces;

namespace Data.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly WebApiDbContext _repoContext;
        private IUserRepository _userRepository;

        public RepositoryFactory(WebApiDbContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public IUserRepository Users
            => _userRepository ??= new UserRepository(_repoContext);

        public void SaveChanges()
        {
            _repoContext.SaveChanges();
        }
    }
}