using Api.Core.Commons;
using System.Linq.Expressions;

namespace Api.Infrastructure.Interface
{
    interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task<bool> AnyAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        Task<int> CountAsync();
        Task<T> GetByIdAsync(object id);
        Task<Pagination<T>> GetAsync(
          Expression<Func<T, bool>> filter,
          int pageIndex = 0,
          int pageSize = 10);
        Task<Pagination<T>> ToPagination(int pageIndex, int pageSize);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task Delete(object id);
        Task DeleteAllAsync();
    }
}