using FluentValidation;
using MediatR;
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
        private readonly IValidator<UpdateEmployeeInput> _validator;

        public UpdateEmployee(IDocumentService documentService, IUnitOfWork unitOfWork, IEmployeeRepository repository, IValidator<UpdateEmployeeInput> validator)
        {
            _documentService = documentService;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<EmployeeOutput> Handle(UpdateEmployeeInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var employee = await _repository.FindByIdAsync(input.Id, cancellationToken);
            employee.Update(input.FirstName, input.LastName, input.Document, input.Email, input.DepartmentId);

            var isDocumentUnique = await _documentService.IsDocumentUnique(employee, cancellationToken);
            if (!isDocumentUnique)
                throw new EntityValidationException("Document must be unique");

            var employeeUpdated = await _repository.UpdateAsync(employee, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return EmployeeOutput.FromEmployee(employeeUpdated);
        }
    }
}
