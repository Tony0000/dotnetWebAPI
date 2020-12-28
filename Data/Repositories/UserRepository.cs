using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Data.Repositories.Interfaces;
using Domain.Model;

namespace Data.Repositories
{
    public class UserRepository : DataRepository<User>, IUserRepository
    {
        private WebApiDbContext _context;

        public UserRepository(WebApiDbContext context) : base(context)
        {
            _context = context;
        }

        public User First(Expression<Func<User, bool>> predicate)
        {
            return Fetch().FirstOrDefault(predicate);
        }

        public override void Delete(User entity)
        {
            entity.Active = false;
        }
    }
}
