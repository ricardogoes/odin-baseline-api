using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Employees.GetEmployeeById
{
    public class GetEmployeeById : IRequestHandler<GetEmployeeByIdInput, EmployeeOutput>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IValidator<GetEmployeeByIdInput> _validator;

        public GetEmployeeById(IEmployeeRepository employeeRepository, IValidator<GetEmployeeByIdInput> validator)
        {
            _employeeRepository = employeeRepository;
            _validator = validator;
        }

        public async Task<EmployeeOutput> Handle(GetEmployeeByIdInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var employee = await _employeeRepository.FindByIdAsync(input.Id, cancellationToken);
            return EmployeeOutput.FromEmployee(employee);
        }
    }
}
