using MediatR;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Departments.ChangeStatusDepartment
{
    public class ChangeStatusDepartment : IRequestHandler<ChangeStatusDepartmentInput, DepartmentOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Department> _repository;

        public ChangeStatusDepartment(IUnitOfWork unitOfWork, IRepository<Department> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<DepartmentOutput> Handle(ChangeStatusDepartmentInput input, CancellationToken cancellationToken)
        {
            var department = await _repository.FindByIdAsync(input.Id, cancellationToken);

            switch (input.Action)
            {
                case ChangeStatusAction.ACTIVATE:
                    department.Activate(input.LoggedUsername);
                    break;
                case ChangeStatusAction.DEACTIVATE:
                    department.Deactivate(input.LoggedUsername);
                    break;
            }

            var departmentUpdated = await _repository.UpdateAsync(department, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return DepartmentOutput.FromDepartment(departmentUpdated);
        }
    }
}
