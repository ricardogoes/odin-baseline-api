using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Models;
using Odin.Infra.Data.Utilities.Expressions;
using Odin.Infra.Data.Utilities.Sort;

namespace Odin.Baseline.Infra.Data.EF.Repositories
{
    public class DepartmentRepository : BaseRepository, IRepository<Department>
    {
        private readonly OdinBaselineDbContext _dbContext;        
        private DbSet<DepartmentModel> Departments => _dbContext.Set<DepartmentModel>();

        public DepartmentRepository(OdinBaselineDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITenantService tenantService)
            : base(httpContextAccessor, tenantService)
        {
            _dbContext = dbContext;
        }

        public async Task<Department> InsertAsync(Department department, CancellationToken cancellationToken)
        {
            var model = department.ToDepartmentModel(GetTenantId());
            model.SetAuditLog(GetCurrentUsername(), created: true);

            var departmentInserted = await Departments.AddAsync(model, cancellationToken) ;     
            
            return departmentInserted.Entity.ToDepartment();
        }

        public async Task<Department> UpdateAsync(Department department, CancellationToken cancellationToken)
        {
            var model = department.ToDepartmentModel(GetTenantId());
            model.SetAuditLog(GetCurrentUsername(), created: false);

            var departmentUpdated = await Task.FromResult(Departments.Update(model));

            return departmentUpdated.Entity.ToDepartment();
        }

        public async Task DeleteAsync(Department department)
            => await Task.FromResult(Departments.Remove(department.ToDepartmentModel(GetTenantId())));

        public async Task<Department> FindByIdAsync(Guid id, CancellationToken cancellationToken) 
        {
            var model = await Departments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            NotFoundException.ThrowIfNull(model, $"Department with Id '{id}' not found.");

            return model!.ToDepartment(); 
        }

        public async Task<PaginatedListOutput<Department>> FindPaginatedListAsync(Dictionary<string, object?> filters, int pageNumber, int pageSize, string sort, CancellationToken cancellationToken) 
        {
            var filtersExpression = ExpressionsUtility<DepartmentModel>.BuildFilterExpression(filters);
            var expression = ExpressionsUtility<DepartmentModel>.BuildQueryableExpression(filtersExpression);

            var data = expression != null 
                ? await Departments.Where(expression).ToListAsync(cancellationToken) 
                : await Departments.ToListAsync(cancellationToken);

            var sortedData = SortUtility.ApplySort(data, sort)!;

            return new PaginatedListOutput<Department>
            (
                totalItems: sortedData.Count(),
                items: sortedData
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToDepartment()
            );
        }
    }
}
