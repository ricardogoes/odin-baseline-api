using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.SeedWork;

namespace Odin.Baseline.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> InsertAsync(T entity, CancellationToken cancellationToken) ;

        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);

        Task DeleteAsync(T entity);

        Task<T> FindByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<PaginatedListOutput<T>> FindPaginatedListAsync(Dictionary<string, object> filters, int pageNumber, int pageSize, string sort, CancellationToken cancellationToken);

        //Task<IEnumerable<Guid>> FindListIdsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);

        //Task<IReadOnlyList<T>> FindListByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
    }
}
