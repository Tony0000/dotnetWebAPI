using System;
using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User First(Expression<Func<User, bool>> predicate);
    }
}
