using MediatR;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.Application.Customers.ChangeAddressCustomer
{
    public class ChangeAddressCustomer : IRequestHandler<ChangeAddressCustomerInput, CustomerOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;

        public ChangeAddressCustomer(IUnitOfWork unitOfWork, ICustomerRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<CustomerOutput> Handle(ChangeAddressCustomerInput request, CancellationToken cancellationToken)
        {
            var customer = await _repository.FindByIdAsync(request.CustomerId, cancellationToken);
            var address = new Address(request.StreetName, request.StreetNumber, request.Complement, request.Neighborhood, request.ZipCode, request.City, request.State);

            customer.ChangeAddress(address);

            await _repository.UpdateAsync(customer);
            await _unitOfWork.CommitAsync(cancellationToken);

            return CustomerOutput.FromCustomer(customer);
        }
    }
}
