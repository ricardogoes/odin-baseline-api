using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.CompaniesPositions;

namespace Odin.Baseline.Domain.Interfaces.Services
{
    public interface ICompaniesPositionsService
    {
        Task<CompanyPositionToQuery> InsertAsync(CompanyPositionToInsert departmentToInsert, string loggedUsername, CancellationToken cancellationToken);
        Task<CompanyPositionToQuery> UpdateAsync(CompanyPositionToUpdate departmentToUpdate, string loggedUsername, CancellationToken cancellationToken);
        Task<CompanyPositionToQuery> ChangeStatusAsync(int departmentId, string loggedUsername, CancellationToken cancellationToken);
        Task<PagedList<CompanyPositionToQuery>> GetByCustomerAsync(int customerId, CompaniesPositionsQueryModel paginationData, CancellationToken cancellationToken);
        Task<CompanyPositionToQuery> GetByIdAsync(int departmentId, CancellationToken cancellationToken);
    }
}
