using InventorySystem.Domain.Common;
using System.Linq.Expressions;

namespace InventorySystem.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        IQueryable<T> GetAll();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true);

        IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true);

        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(int id, List<Expression<Func<T, object>>> includes = null, bool disableTracking = false);

        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
