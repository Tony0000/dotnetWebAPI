using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        DbContext CurrentDbContext { get; }
        IQueryable<T> Fetch(bool track);
        IQueryable<T> GetAll();
        T Find(int id);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool track);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool track);
        Task<IEnumerable<T>> GetAllAsync(bool track = true);
        Task<T> GetAsync(int id, bool track = true);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Attach(T entity);
        void Detach(T entity);
        void SaveChanges();
        public bool Exists(Expression<Func<T, bool>> predicate);
    }
}
