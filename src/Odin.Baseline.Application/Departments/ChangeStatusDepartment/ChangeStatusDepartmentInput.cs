using MediatR;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Departments.ChangeStatusDepartment
{
    public class ChangeStatusDepartmentInput : IRequest<DepartmentOutput>
    {        
        public Guid Id { get; private set; }
        public ChangeStatusAction? Action { get; private set; }

        public ChangeStatusDepartmentInput(Guid id, ChangeStatusAction? action)
        {
            Id = id;
            Action = action;
        }
    }
}
