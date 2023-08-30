using MediatR;
using Odin.Baseline.Application.Customers.Common;

namespace Odin.Baseline.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerInput : IRequest<CustomerOutput>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string LoggedUsername { get; set; }
    }
}
