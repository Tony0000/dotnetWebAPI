﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Application.Common.Interfaces;
using Application.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : DataRepository<User>, IUserRepository
    {
        private IWebApiDbContext _context;

        public UserRepository(IWebApiDbContext context) : base(context)
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
