using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Employees.AddPosition
{
    public class AddPosition : IRequestHandler<AddPositionInput, EmployeeOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _repository;
        private readonly IValidator<AddPositionInput> _validator;

        public AddPosition(IUnitOfWork unitOfWork, IEmployeeRepository repository, IValidator<AddPositionInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<EmployeeOutput> Handle(AddPositionInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var employee = await _repository.FindByIdAsync(input.EmployeeId, cancellationToken);

            employee.AddHistoricPosition(new EmployeePositionHistory(input.PositionId, input.Salary, input.StartDate, input.FinishDate), input.LoggedUsername);

            var employeeUpdated = await _repository.UpdateAsync(employee, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return EmployeeOutput.FromEmployee(employeeUpdated);
        }
    }
}
