using MediatR;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Departments.ChangeStatusDepartment
{
    public class ChangeStatusDepartmentInput : IRequest<DepartmentOutput>
    {
        public Guid Id { get; set; }
        public ChangeStatusAction? Action { get; set; }
        public string LoggedUsername { get; set; }
    }
}
