namespace Odin.Baseline.Domain.Interfaces.Repositories
{
    public interface ISortHelper
    {
        IQueryable<T> ApplySort<T>(IQueryable<T> data, string orderByQueryString) where T : class;
    }
}
