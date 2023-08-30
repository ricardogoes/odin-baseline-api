using Odin.Baseline.Domain.SeedWork;

namespace Odin.Baseline.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {        
        Task CommitAsync(CancellationToken cancellationToken);
    }
}
