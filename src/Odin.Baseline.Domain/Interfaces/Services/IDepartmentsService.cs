using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Departments;

namespace Odin.Baseline.Domain.Interfaces.Services
{
    public interface IDepartmentsService
    {
        Task<DepartmentToQuery> InsertAsync(DepartmentToInsert departmentToInsert, string loggedUsername, CancellationToken cancellationToken);
        Task<DepartmentToQuery> UpdateAsync(DepartmentToUpdate departmentToUpdate, string loggedUsername, CancellationToken cancellationToken);
        Task<DepartmentToQuery> ChangeStatusAsync(int departmentId, string loggedUsername, CancellationToken cancellationToken);
        Task<PagedList<DepartmentToQuery>> GetByCustomerAsync(int customerId, DepartmentsQueryModel paginationData, CancellationToken cancellationToken);
        Task<DepartmentToQuery> GetByIdAsync(int departmentId, CancellationToken cancellationToken);
    }
}
