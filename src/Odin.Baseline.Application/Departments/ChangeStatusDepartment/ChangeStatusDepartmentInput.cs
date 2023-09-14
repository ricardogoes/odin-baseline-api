using MediatR;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Departments.ChangeStatusDepartment
{
    public class ChangeStatusDepartmentInput : IRequest<DepartmentOutput>
    {        
        public Guid Id { get; private set; }
        public ChangeStatusAction? Action { get; private set; }
        public string LoggedUsername { get; private set; }

        public ChangeStatusDepartmentInput(Guid id, ChangeStatusAction? action, string loggedUsername)
        {
            Id = id;
            Action = action;
            LoggedUsername = loggedUsername;
        }
    }
}
