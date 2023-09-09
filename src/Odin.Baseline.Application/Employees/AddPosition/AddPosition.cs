using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Employees.AddPosition
{
    public class AddPosition : IRequestHandler<AddPositionInput, EmployeeOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _repository;

        public AddPosition(IUnitOfWork unitOfWork, IEmployeeRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<EmployeeOutput> Handle(AddPositionInput input, CancellationToken cancellationToken)
        {
            var employee = await _repository.FindByIdAsync(input.EmployeeId, cancellationToken);

            employee.AddHistoricPosition(new EmployeePositionHistory(input.PositionId, input.Salary, input.StartDate, input.FinishDate));

            var employeeUpdated = await _repository.UpdateAsync(employee, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return EmployeeOutput.FromEmployee(employeeUpdated);
        }
    }
}
