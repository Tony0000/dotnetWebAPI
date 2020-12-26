using System;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        DbContext CurrentDbContext { get; }
        IQueryable<T> Fetch();
        IQueryable<T> GetAll();
        T Find(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Attach(T entity);
        void Detach(T entity);
        void SaveChanges();
        public bool Exists(Expression<Func<T, bool>> predicate);
    }
}
