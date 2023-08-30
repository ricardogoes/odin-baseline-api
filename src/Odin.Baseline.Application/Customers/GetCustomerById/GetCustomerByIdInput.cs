using MediatR;
using Odin.Baseline.Application.Customers.Common;

namespace Odin.Baseline.Application.Customers.GetCustomerById
{
    public class GetCustomerByIdInput : IRequest<CustomerOutput>
    {
        public Guid Id { get; set; }
    }
}
