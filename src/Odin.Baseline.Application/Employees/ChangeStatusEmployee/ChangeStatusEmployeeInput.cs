using MediatR;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Employees.ChangeStatusEmployee
{
    public class ChangeStatusEmployeeInput : IRequest<EmployeeOutput>
    {
        public Guid Id { get; private set; }
        public ChangeStatusAction? Action { get; private set; }

        public ChangeStatusEmployeeInput(Guid id, ChangeStatusAction? action)
        {
            Id = id;
            Action = action;
        }
    }
}
