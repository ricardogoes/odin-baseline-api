using MediatR;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Customers.ChangeStatusCustomer
{
    public class ChangeStatusCustomerInput : IRequest<CustomerOutput>
    {
        public Guid Id { get; private set; }
        public ChangeStatusAction? Action { get; private set; }
        public string LoggedUsername { get; private set; }

        public ChangeStatusCustomerInput(Guid id, ChangeStatusAction? action, string loggedUsername)
        {
            Id = id;
            Action = action;
            LoggedUsername = loggedUsername;
        }
    }
}
