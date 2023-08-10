using Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.Domain.Interfaces.Repositories
{
    public interface ICustomersRepository
    {
        Task<Customer> InsertAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<Customer> ChangeStatusAsync(Customer customer);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int customerId);
    }
}
