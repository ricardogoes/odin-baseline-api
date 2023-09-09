using MediatR;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Customers.ChangeStatusCustomer
{
    public class ChangeStatusCustomer : IRequestHandler<ChangeStatusCustomerInput, CustomerOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;

        public ChangeStatusCustomer(IUnitOfWork unitOfWork, ICustomerRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<CustomerOutput> Handle(ChangeStatusCustomerInput input, CancellationToken cancellationToken)
        {
            var customer = await _repository.FindByIdAsync(input.Id, cancellationToken);

            switch (input.Action)
            {
                case ChangeStatusAction.ACTIVATE:
                    customer.Activate(input.LoggedUsername);
                    break;
                case ChangeStatusAction.DEACTIVATE:
                    customer.Deactivate(input.LoggedUsername);
                    break;
            }

            await _repository.UpdateAsync(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return CustomerOutput.FromCustomer(customer);
        }
    }
}
