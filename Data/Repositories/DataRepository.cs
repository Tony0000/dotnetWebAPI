using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Repositories.Interfaces;
using Domain.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class DataRepository<T> : RepositoryExtension<T>, IRepository<T> 
        where T : BaseEntity
    {
        private DbContext _context;
        protected DbSet<T> _objectSet;

        public DbContext CurrentDbContext => _context;

        DbContext IRepository<T>.CurrentDbContext => CurrentDbContext;

        protected DataRepository(DbContext context)
        {
            _context = context;
            _objectSet = _context.Set<T>();
        }

        public IQueryable<T> Fetch(bool track = true)
        {
            var query = _objectSet.Include(x => x.UpdatedBy).Include(x => x.CreatedBy);
            return track ? query : query.AsNoTracking();
        }

        public virtual IQueryable<T> GetAll(bool track = false)
        {
            return Fetch(track);
        }

        public virtual T Find(int id)
        {
            return Fetch().FirstOrDefault(x => x.Id == id);
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
            return await Find(x => x.Id == id).FirstOrDefaultAsync();
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

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Cannot store a null entity");

            var id = entity.Id;
            var model = Find(id);
            _context.Entry(model).CurrentValues.SetValues(entity);
            _context.Entry(model).State = EntityState.Modified;
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

        public void Attach(T entity)
        {
            _objectSet.Attach(entity);
        }

        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!disposing || _context == null) return;
            _context.Dispose();
            _context = null;
        }
    }
}
