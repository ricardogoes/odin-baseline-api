using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Employees.CreateEmployee
{
    public class CreateEmployee : IRequestHandler<CreateEmployeeInput, EmployeeOutput>
    {
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _repository;
        private readonly IValidator<CreateEmployeeInput> _validator;

        public CreateEmployee(IDocumentService documentService, IUnitOfWork unitOfWork, IEmployeeRepository repository, IValidator<CreateEmployeeInput> validator)
        {
            _documentService = documentService;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<EmployeeOutput> Handle(CreateEmployeeInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var employee = new Employee(input.CustomerId, input.FirstName, input.LastName, input.Document, input.Email, input.DepartmentId, isActive: true);
            employee.Create(input.LoggedUsername);

            var isDocumentUnique = await _documentService.IsDocumentUnique(employee, cancellationToken);
            if (!isDocumentUnique)
                throw new EntityValidationException("Document must be unique");

            await _repository.InsertAsync(employee, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return EmployeeOutput.FromEmployee(employee);
        }
    }
}
