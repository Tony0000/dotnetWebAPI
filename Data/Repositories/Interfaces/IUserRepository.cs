using System;
using System.Linq.Expressions;
using Domain.Model;

namespace Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User First(Expression<Func<User, bool>> predicate);
    }
}
