using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Employees.ChangeStatusEmployee
{
    public class ChangeStatusEmployeeInput : IRequest<EmployeeOutput>
    {
        public Guid Id { get; set; }
        public ChangeStatusAction? Action { get; set; }
        public string LoggedUsername { get; set; }
    }
}
