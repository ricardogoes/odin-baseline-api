using MediatR;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Customers.GetCustomerById
{
    public class GetCustomerById : IRequestHandler<GetCustomerByIdInput, CustomerOutput>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerById(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerOutput> Handle(GetCustomerByIdInput input, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.FindByIdAsync(input.Id, cancellationToken);
            return CustomerOutput.FromCustomer(customer);
        }
    }
}
