using MediatR;
using Odin.Baseline.Application.Departments.Common;

namespace Odin.Baseline.Application.Departments.CreateDepartment
{
    public class CreateDepartmentInput : IRequest<DepartmentOutput>
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string LoggedUsername { get; set; }
    }
}
