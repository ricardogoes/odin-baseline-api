using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.SeedWork;

namespace Odin.Baseline.Domain.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public DocumentService(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository)
        {
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> IsDocumentUnique(EntityWithDocument entity, CancellationToken cancellationToken)
        {
            if (entity is Customer)
            {
                try
                {
                    var customer = await _customerRepository.FindByDocumentAsync(entity.Document, cancellationToken);
                    return customer.Id == entity.Id;
                }
                catch (NotFoundException)
                {
                    return true;
                }                
            }
            else if (entity is Employee)
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
            else
                throw new ArgumentException("Invalid entity", nameof(entity));
            
        }
    }
}
