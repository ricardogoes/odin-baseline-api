using Odin.Baseline.Domain.Models;
using System.Linq.Expressions;

namespace Odin.Baseline.Domain.Interfaces.Repositories
{
    public interface IRepository
    {
        Task<TResult> GetByIdAsync<T, TResult>(int id, CancellationToken cancellationToken) 
            where T : class 
            where TResult : class;
        
        Task<PagedList<TResult>> FindListAsync<T, TResult>(Expression<Func<T, bool>> expression, int pageNumber, int pageSize, string sort, CancellationToken cancellationToken = default)
            where T : class
            where TResult : class;

        Task<PagedList<TResult>> FindAllAsync<T, TResult>(int pageNumber, int pageSize, string sort, CancellationToken cancellationToken) 
            where T : class
            where TResult: class;

        Task<TResult> SingleOrDefaultAsync<T, TResult>(Expression<Func<T, bool>> expression, string includeProperties) 
            where T : class
            where TResult: class;

        TResult Insert<T, TResult>(T entity)
            where T : class
            where TResult : class;

        TResult Update<T, TResult>(T entity)
            where T : class
            where TResult : class;

        void UpdateRange<T>(IEnumerable<T> entities) where T : class;
        void Delete<T>(T entity) where T : class;
    }
}
