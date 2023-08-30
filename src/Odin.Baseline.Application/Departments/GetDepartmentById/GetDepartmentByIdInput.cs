using MediatR;
using Odin.Baseline.Application.Departments.Common;

namespace Odin.Baseline.Application.Departments.GetDepartmentById
{
    public class GetDepartmentByIdInput : IRequest<DepartmentOutput>
    {
        public Guid Id { get; set; }
    }
}
