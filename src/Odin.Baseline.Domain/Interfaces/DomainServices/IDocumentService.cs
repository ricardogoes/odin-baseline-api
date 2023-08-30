using Odin.Baseline.Domain.SeedWork;

namespace Odin.Baseline.Domain.Interfaces.DomainServices
{
    public interface IDocumentService
    {
        Task<bool> IsDocumentUnique(EntityWithDocument entity, CancellationToken cancellationToken);
    }
}
