using Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> FindByDocumentAsync(string document, CancellationToken cancellationToken);
    }
}
