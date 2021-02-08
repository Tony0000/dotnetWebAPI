using System;
using System.Linq.Expressions;
using Domain.Entities;

namespace Persistence.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User First(Expression<Func<User, bool>> predicate);
    }
}
