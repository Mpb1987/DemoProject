using System.Linq.Expressions;
using DemoProject.ApplicationCore.Interfaces;
using DemoProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DemoProject.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ILogger _log;
        protected readonly DemoContext _dbContext;

        public BaseRepository(ILogger<T> log, DemoContext dbContext)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        
        public async Task<T> GetSingleBySpecAsync(Expression<Func<T, bool>> predicate)
        {
            var queryWithResults = _dbContext.Set<T>().AsQueryable();

            var result = await queryWithResults.Where(predicate).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            var dbSet = _dbContext.Set<T>();

            return await dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = GetQueryWithIncludes(includes);

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByReadOnlyAsync(Expression<Func<T, bool>> predicate)
        {
            var dbSet = _dbContext.Set<T>().AsNoTracking();

            return await dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByReadOnlyAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = GetQueryWithIncludes(includes);

            return await query.Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllReadOnlyAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = GetQueryWithIncludes(includes);
            return await query.ToListAsync();
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                await _dbContext.Set<T>().AddAsync(entity);

                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> entity)
        {
            await _dbContext.Set<T>().AddRangeAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<T> GetQueryWithIncludes(Expression<Func<T, object>>[] includes)
        {
            var dbSet = _dbContext.Set<T>();
            var query = dbSet.AsQueryable();

            return includes.Aggregate(query, (current, include) => current.Include(include));
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public IEnumerable<int> GetNumericFieldBy(Func<T, int> selector, Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).ToList().Select(selector);
        }

        public async Task<bool>? CheckExistsByPredicate(Expression<Func<T, bool>> predicate)
        {
            var dbSet = _dbContext.Set<T>();
            var exists = await dbSet.AnyAsync(predicate);
            return exists;
        }
    }
}
