using MediatR;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Customers.UpdateCustomer
{
    public class UpdateCustomer : IRequestHandler<UpdateCustomerInput, CustomerOutput>
    {
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;

        public UpdateCustomer(IDocumentService documentService, IUnitOfWork unitOfWork, ICustomerRepository repository)
        {
            _documentService = documentService;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<CustomerOutput> Handle(UpdateCustomerInput input, CancellationToken cancellationToken)
        {
            var customer = await _repository.FindByIdAsync(input.Id, cancellationToken);
            customer.Update(input.Name, input.Document, input.LoggedUsername);

            var isDocumentUnique = await _documentService.IsDocumentUnique(customer, cancellationToken);
            if (!isDocumentUnique)
                throw new EntityValidationException("Document must be unique");

            await _repository.UpdateAsync(customer);
            await _unitOfWork.CommitAsync(cancellationToken);

            return CustomerOutput.FromCustomer(customer);
        }
    }
}
