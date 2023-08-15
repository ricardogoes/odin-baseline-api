using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Employees;

namespace Odin.Baseline.Domain.Interfaces.Services
{
    public interface IEmployeesService
    {
        Task<EmployeeToQuery> InsertAsync(EmployeeToInsert employeeToInsert, string loggedUsername, CancellationToken cancellationToken);
        Task<EmployeeToQuery> UpdateAsync(EmployeeToUpdate employeeToUpdate, string loggedUsername, CancellationToken cancellationToken);
        Task<EmployeeToQuery> ChangeStatusAsync(int employeeId, string loggedUsername, CancellationToken cancellationToken);
        Task<EmployeeToQuery> GetByIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<PagedList<EmployeeToQuery>> GetByCustomerAsync(int customerId, EmployeesQueryModel paginationData, CancellationToken cancellationToken);
        Task<PagedList<EmployeeToQuery>> GetByDepartmentAsync(int departmentId, EmployeesQueryModel paginationData, CancellationToken cancellationToken);
        Task<PagedList<EmployeeToQuery>> GetByCompanyPositionAsync(int positionId, EmployeesQueryModel paginationData, CancellationToken cancellationToken);
    }
}
