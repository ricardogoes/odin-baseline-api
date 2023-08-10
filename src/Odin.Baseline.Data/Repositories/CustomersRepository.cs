using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Data.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        public async Task<Customer> ChangeStatusAsync(Customer customer)
        {
            return await Task.FromResult(new Customer
            {
                CustomerId = 1,
                Name = "Cliente 01",
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "ricardo.goes",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "ricardo.goes"
            });
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await Task.FromResult(new List<Customer>
            {
                new Customer { CustomerId = 1, Name = "Cliente 01", IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "ricardo.goes", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "ricardo.goes" },
                new Customer { CustomerId = 2, Name = "Cliente 02", IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "ricardo.goes", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "ricardo.goes" },
                new Customer { CustomerId = 3, Name = "Cliente 03", IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "ricardo.goes", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "ricardo.goes" }
            });
        }

        public async Task<Customer> GetByIdAsync(int customerId)
        {
            return await Task.FromResult(new Customer 
            { 
                CustomerId = 1, 
                Name = "Cliente 01", 
                IsActive = true, 
                CreatedAt = DateTime.Now, 
                CreatedBy = "ricardo.goes", 
                LastUpdatedAt = DateTime.Now, 
                LastUpdatedBy = "ricardo.goes" 
            });
        }

        public async Task<Customer> InsertAsync(Customer customer)
        {
            return await Task.FromResult(new Customer
            {
                CustomerId = 1,
                Name = "Cliente 01",
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "ricardo.goes",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "ricardo.goes"
            });
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            return await Task.FromResult(new Customer
            {
                CustomerId = 1,
                Name = "Cliente 01",
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "ricardo.goes",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "ricardo.goes"
            });
        }
    }
}
