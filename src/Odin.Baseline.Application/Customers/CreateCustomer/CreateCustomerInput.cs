using MediatR;
using Odin.Baseline.Application.Customers.Common;

namespace Odin.Baseline.Application.Customers.CreateCustomer
{
    public class CreateCustomerInput : IRequest<CustomerOutput>
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string LoggedUsername { get; set; }
    }
}
