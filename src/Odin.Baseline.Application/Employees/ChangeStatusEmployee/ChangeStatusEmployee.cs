using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Employees.ChangeStatusEmployee
{
    public class ChangeStatusEmployee : IRequestHandler<ChangeStatusEmployeeInput, EmployeeOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _repository;
        private readonly IValidator<ChangeStatusEmployeeInput> _validator;

        public ChangeStatusEmployee(IUnitOfWork unitOfWork, IEmployeeRepository repository, IValidator<ChangeStatusEmployeeInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<EmployeeOutput> Handle(ChangeStatusEmployeeInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var employee = await _repository.FindByIdAsync(input.Id, cancellationToken);

            switch (input.Action)
            {
                case ChangeStatusAction.ACTIVATE:
                    employee.Activate(input.LoggedUsername);
                    break;
                case ChangeStatusAction.DEACTIVATE:
                    employee.Deactivate(input.LoggedUsername);
                    break;
            }

            var employeeUpdated = await _repository.UpdateAsync(employee, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return EmployeeOutput.FromEmployee(employeeUpdated);
        }
    }
}
