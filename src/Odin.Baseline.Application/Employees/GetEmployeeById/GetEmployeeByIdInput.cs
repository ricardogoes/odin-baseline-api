using MediatR;
using Odin.Baseline.Application.Employees.Common;

namespace Odin.Baseline.Application.Employees.GetEmployeeById
{
    public class GetEmployeeByIdInput : IRequest<EmployeeOutput>
    {
        public Guid Id { get; set; }
    }
}
