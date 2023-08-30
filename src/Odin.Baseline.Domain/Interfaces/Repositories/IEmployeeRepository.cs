using Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.Domain.Interfaces.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> FindByDocumentAsync(string document, CancellationToken cancellationToken);
    }
}
