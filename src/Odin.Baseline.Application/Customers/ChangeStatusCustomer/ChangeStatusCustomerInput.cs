using MediatR;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Customers.ChangeStatusCustomer
{
    public class ChangeStatusCustomerInput : IRequest<CustomerOutput>
    {
        public Guid Id { get; set; }
        public ChangeStatusAction? Action { get; set; }
        public string LoggedUsername { get; set; }
    }
}
