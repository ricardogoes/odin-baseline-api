using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Employees.GetEmployeeById
{
    public class GetEmployeeById : IRequestHandler<GetEmployeeByIdInput, EmployeeOutput>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeById(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeOutput> Handle(GetEmployeeByIdInput input, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.FindByIdAsync(input.Id, cancellationToken);
            return EmployeeOutput.FromEmployee(employee);
        }
    }
}
