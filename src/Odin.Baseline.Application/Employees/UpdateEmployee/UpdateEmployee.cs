using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Employees.UpdateEmployee
{
    public class UpdateEmployee : IRequestHandler<UpdateEmployeeInput, EmployeeOutput>
    {
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _repository;

        public UpdateEmployee(IDocumentService documentService, IUnitOfWork unitOfWork, IEmployeeRepository repository)
        {
            _documentService = documentService;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<EmployeeOutput> Handle(UpdateEmployeeInput input, CancellationToken cancellationToken)
        {
            var employee = await _repository.FindByIdAsync(input.Id, cancellationToken);
            employee.Update(input.FirstName, input.LastName, input.Document, input.Email, input.CustomerId, input.DepartmentId, input.LoggedUsername);

            var isDocumentUnique = await _documentService.IsDocumentUnique(employee, cancellationToken);
            if (!isDocumentUnique)
                throw new EntityValidationException("Document must be unique");

            var employeeUpdated = await _repository.UpdateAsync(employee, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return EmployeeOutput.FromEmployee(employeeUpdated);
        }
    }
}
