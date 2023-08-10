using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ViewModels.Customers;

namespace Odin.Baseline.Domain.Interfaces.Services
{
    public interface ICustomersService
    {
        Task<Customer> InsertAsync(CustomerToInsert customerToInsert);
        Task<Customer> UpdateAsync(CustomerToUpdate customerToUpdate);
        Task<Customer> ChangeStatusAsync(int customerId);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int customerId);
    }
}
