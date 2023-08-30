using MediatR;
using Odin.Baseline.Application.Customers.Common;

namespace Odin.Baseline.Application.Customers.ChangeAddressCustomer
{
    public class ChangeAddressCustomerInput : IRequest<CustomerOutput>
    {
        public Guid CustomerId { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
