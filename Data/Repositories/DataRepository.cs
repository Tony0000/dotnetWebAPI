using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Interfaces;

namespace Persistence.Repositories
{
    public class DataRepository<T> : RepositoryExtension<T>, IRepository<T> 
        where T : class
    {
        private readonly IWebApiDbContext _context;
        protected DbSet<T> _objectSet;

        public IWebApiDbContext CurrentDbContext => _context;

        IWebApiDbContext IRepository<T>.CurrentDbContext => CurrentDbContext;

        protected DataRepository(IWebApiDbContext context)
        {
            _context = context;
            _objectSet = _context.Set<T>();
        }

        public IQueryable<T> Fetch(bool track = true)
        {
            return track ? _objectSet : _objectSet.AsNoTracking();
        }

        public virtual IQueryable<T> GetAll(bool track = false)
        {
            return Fetch(track);
        }

        public virtual T Find(int id)
        {
            return Fetch().FirstOrDefaultDynamic(x => $"x.Id == {id}");
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool track = true)
        {
            return Fetch(track).Where(predicate);
        }

        public T First(Expression<Func<T, bool>> predicate, string relatedEntity = null)
        {
            return string.IsNullOrEmpty(relatedEntity) ?
                Fetch().FirstOrDefault(predicate)
                : Fetch().Include(relatedEntity).FirstOrDefault(predicate);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool track)
        {
            return await Find(predicate, track).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool track = false)
        {
            return await Fetch(track).ToListAsync();
        }


        public async Task<T> GetAsync(int id, bool track = true)
        {
            return await Fetch(track).WhereDynamic(x => $"x.Id = {id}").FirstOrDefaultAsync();
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _objectSet.Any(predicate);
        }

        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Cannot store a null entity");

            _objectSet.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Cannot store a null entity");

            _objectSet.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var records = from x in _objectSet.Where(predicate) select x;

            foreach (var record in records)
                _objectSet.Remove(record);
        }
    }
}
