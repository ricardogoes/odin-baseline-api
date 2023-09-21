using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Customers.CreateCustomer
{
    public class CreateCustomer : IRequestHandler<CreateCustomerInput, CustomerOutput>
    {
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;        
        private readonly ICustomerRepository _repository;
        private readonly IValidator<CreateCustomerInput> _validator;

        public CreateCustomer(IDocumentService documentService, IUnitOfWork unitOfWork, ICustomerRepository repository, IValidator<CreateCustomerInput> validator)
        {
            _documentService = documentService;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<CustomerOutput> Handle(CreateCustomerInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var customer = new Customer(input.Name, input.Document, isActive: true);
            customer.Create(input.LoggedUsername);

            var isDocumentUnique = await _documentService.IsDocumentUnique(customer, cancellationToken);
            if (!isDocumentUnique)
                throw new EntityValidationException("Document must be unique");

            await _repository.InsertAsync(customer, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return CustomerOutput.FromCustomer(customer);
        }
    }
}
