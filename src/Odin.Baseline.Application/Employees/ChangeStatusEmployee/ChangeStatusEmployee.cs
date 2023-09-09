using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Employees.ChangeStatusEmployee
{
    public class ChangeStatusEmployee : IRequestHandler<ChangeStatusEmployeeInput, EmployeeOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _repository;

        public ChangeStatusEmployee(IUnitOfWork unitOfWork, IEmployeeRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<EmployeeOutput> Handle(ChangeStatusEmployeeInput input, CancellationToken cancellationToken)
        {
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
