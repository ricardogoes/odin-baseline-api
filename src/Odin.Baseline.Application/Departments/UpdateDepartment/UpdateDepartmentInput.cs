using MediatR;
using Odin.Baseline.Application.Departments.Common;

namespace Odin.Baseline.Application.Departments.UpdateDepartment
{
    public class UpdateDepartmentInput : IRequest<DepartmentOutput>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string LoggedUsername { get; set; }
    }
}
