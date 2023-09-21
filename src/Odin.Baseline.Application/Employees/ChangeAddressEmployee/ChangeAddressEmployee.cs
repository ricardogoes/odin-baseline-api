using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.Application.Employees.ChangeAddressEmployee
{
    public class ChangeAddressEmployee : IRequestHandler<ChangeAddressEmployeeInput, EmployeeOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _repository;
        private readonly IValidator<ChangeAddressEmployeeInput> _validator;

        public ChangeAddressEmployee(IUnitOfWork unitOfWork, IEmployeeRepository repository, IValidator<ChangeAddressEmployeeInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<EmployeeOutput> Handle(ChangeAddressEmployeeInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var employee = await _repository.FindByIdAsync(input.EmployeeId, cancellationToken);
            var address = new Address(input.StreetName, input.StreetNumber, input.Complement, input.Neighborhood, input.ZipCode, input.City, input.State);

            employee.ChangeAddress(address, input.LoggedUsername);

            var employeeUpdated = await _repository.UpdateAsync(employee, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return EmployeeOutput.FromEmployee(employeeUpdated);
        }
    }
}
