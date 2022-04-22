using System.Linq.Expressions;

namespace DemoProject.ApplicationCore.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetSingleBySpecAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindByReadOnlyAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindByReadOnlyAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<List<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entity);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        IQueryable<T> GetAll();
        IEnumerable<int> GetNumericFieldBy(Func<T, int> selector, Expression<Func<T, bool>> predicate);
        Task<bool>? CheckExistsByPredicate(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllReadOnlyAsync(params Expression<Func<T, object>>[] includes);

    }
}
