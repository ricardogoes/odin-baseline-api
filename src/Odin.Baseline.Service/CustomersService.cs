using AutoMapper;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Domain.ViewModels.Customers;

namespace Odin.Baseline.Service
{
    public class CustomersService : ICustomersService
    {
        private readonly ICustomersRepository _repository;
        private readonly IMapper _mapper;

        public CustomersService(ICustomersRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Customer> InsertAsync(CustomerToInsert customerToInsert)
        {
            var customer = _mapper.Map<Customer>(customerToInsert);
            customer.IsActive = true;
            customer.CreatedBy = "";
            customer.CreatedAt = DateTime.Now;
            customer.LastUpdatedBy = "";
            customer.LastUpdatedAt = DateTime.Now;

            return await _repository.InsertAsync(customer);
        }

        public async Task<Customer> UpdateAsync(CustomerToUpdate customerToUpdate)
        {
            var searchedCustomer = await GetByIdAsync(customerToUpdate.CustomerId);
            if (searchedCustomer is null)
                throw new NotFoundException("Customer not found");

            var customer = _mapper.Map<Customer>(customerToUpdate);
            customer.IsActive = true;            
            customer.LastUpdatedBy = "";
            customer.LastUpdatedAt = DateTime.Now;

            return await _repository.UpdateAsync(customer);
        }

        public async Task<Customer> ChangeStatusAsync(int customerId)
        {
            var customer = await GetByIdAsync(customerId);
            if (customer is null)
                throw new NotFoundException("Customer not found");

            customer.IsActive = !customer.IsActive;
            customer.LastUpdatedBy = "";
            customer.LastUpdatedAt = DateTime.Now;

            return await _repository.ChangeStatusAsync(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Customer> GetByIdAsync(int customerId)
            => await _repository.GetByIdAsync(customerId);
    }
}
