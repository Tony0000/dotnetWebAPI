using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.Interfaces;

namespace Persistence.Repositories.Interfaces
{
    public interface IRepository<T> : IRepositoryExtension<T> where T : class
    {
        IWebApiDbContext CurrentDbContext { get; }
        IQueryable<T> Fetch(bool track);
        IQueryable<T> GetAll(bool track);
        T Find(int id);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool track);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool track);
        Task<IEnumerable<T>> GetAllAsync(bool track = true);
        Task<T> GetAsync(int id, bool track = true);
        void Add(T entity);
        void Delete(T entity);
        public bool Exists(Expression<Func<T, bool>> predicate);
    }
}
