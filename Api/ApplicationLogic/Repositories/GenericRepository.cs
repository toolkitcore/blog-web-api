using Api.Core.Commons;
using Api.Infrastructure.Interface;
using Api.Infrastructure.Persistence;
using Api.Presentation.Constants;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.ApplicationLogic.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);

        #region  Read

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
            => await _dbSet.AnyAsync(filter);

        public async Task<bool> AnyAsync()
            => await _dbSet.AnyAsync();

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
                return await _dbSet.CountAsync();

            return await _dbSet.CountAsync(filter);
        }

        public async Task<int> CountAsync()
            => await _dbSet.CountAsync();

        public async Task<T> GetByIdAsync(object id)
            => await _dbSet.FindAsync(id)
            ?? throw new ArgumentNullException(ErrorConstants.NotFoundMessage);

        public async Task<Pagination<T>> ToPagination(int pageIndex, int pageSize)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<T>> GetAsync(
            Expression<Func<T, bool>> filter,
            int pageIndex = 0,
            int pageSize = 10)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.Where(filter)
                                    .Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<T>> GetAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IQueryable<T>> include = null,
            int pageIndex = 0,
            int pageSize = 10)
        {
            var query = _dbSet.AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var itemCount = await query.CountAsync();
            var items = await query.Skip(pageIndex * pageSize)
                                  .Take(pageSize)
                                  .AsNoTracking()
                                  .ToListAsync();

            var result = new Pagination<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            var query = _dbSet.IgnoreQueryFilters()
                              .AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                return await query.FirstOrDefaultAsync(filter)
                       ?? throw new ArgumentNullException(ErrorConstants.NotFoundMessage);
            }
            else
            {
                return await query.FirstOrDefaultAsync()
                       ?? throw new ArgumentNullException(ErrorConstants.NotFoundMessage);
            }
        }

        #endregion
        #region Update & delete

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void UpdateRange(IEnumerable<T> entities)
            => _dbSet.UpdateRange(entities);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public void DeleteRange(IEnumerable<T> entities)
            => _dbSet.RemoveRange(entities);

        public async Task DeleteAllAsync()
            => await _dbSet.ExecuteDeleteAsync();

        public async Task Delete(object id)
        {
            T entity = await GetByIdAsync(id);
            Delete(entity);
        }
        #endregion
    }
}