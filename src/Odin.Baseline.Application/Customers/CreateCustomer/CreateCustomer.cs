﻿using MediatR;
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

        public CreateCustomer(IDocumentService documentService, IUnitOfWork unitOfWork, ICustomerRepository repository)
        {
            _documentService = documentService;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<CustomerOutput> Handle(CreateCustomerInput input, CancellationToken cancellationToken)
        {
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