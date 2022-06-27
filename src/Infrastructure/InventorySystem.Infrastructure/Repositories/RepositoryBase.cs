using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Domain.Common;
using InventorySystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InventorySystem.Domain.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>()
                .Where(predicate);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, 
            IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (predicate != null) query = query.Where(predicate);

            if (!string.IsNullOrEmpty(includeString)) query = query.Include(includeString);

            if (orderBy != null) return  orderBy(query);

            if (disableTracking) query = query.AsNoTracking();

            return query;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, 
            IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (predicate != null) query = query.Where(predicate);

            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (orderBy != null) return orderBy(query);

            if (disableTracking) query = query.AsNoTracking();

            return query;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(int id, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> entity =  _context.Set<T>();

            if (includes != null) entity = includes.Aggregate(entity, (current, include) => current.Include(include));

            if (disableTracking) entity = entity.AsNoTracking();

            return await entity.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }        

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
