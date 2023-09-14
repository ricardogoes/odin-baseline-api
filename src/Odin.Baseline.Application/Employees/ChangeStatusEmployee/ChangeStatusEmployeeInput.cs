using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Employees.ChangeStatusEmployee
{
    public class ChangeStatusEmployeeInput : IRequest<EmployeeOutput>
    {
        public Guid Id { get; private set; }
        public ChangeStatusAction? Action { get; private set; }
        public string LoggedUsername { get; private set; }

        public ChangeStatusEmployeeInput(Guid id, ChangeStatusAction? action, string loggedUsername)
        {
            Id = id;
            Action = action;
            LoggedUsername = loggedUsername;
        }
    }
}
