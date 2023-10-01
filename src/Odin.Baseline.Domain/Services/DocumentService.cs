using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.SeedWork;

namespace Odin.Baseline.Domain.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public DocumentService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> IsDocumentUnique(EntityWithDocument entity, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.FindByDocumentAsync(entity.Document, cancellationToken);
                return employee.Id == entity.Id;
            }
            catch (NotFoundException)
            {
                return true;
            }
        }
    }
}
