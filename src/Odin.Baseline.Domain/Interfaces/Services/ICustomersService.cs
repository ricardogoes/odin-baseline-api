using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Customers;

namespace Odin.Baseline.Domain.Interfaces.Services
{
    public interface ICustomersService
    {
        Task<Customer> InsertAsync(CustomerToInsert customerToInsert, string loggedUsername, CancellationToken cancellationToken);
        Task<Customer> UpdateAsync(CustomerToUpdate customerToUpdate, string loggedUsername, CancellationToken cancellationToken);
        Task<Customer> ChangeStatusAsync(int customerId, string loggedUsername, CancellationToken cancellationToken);
        Task<PagedList<Customer>> GetAllAsync(CustomersQueryModel paginationData, CancellationToken cancellationToken);
        Task<Customer> GetByIdAsync(int customerId, CancellationToken cancellationToken);
    }
}
