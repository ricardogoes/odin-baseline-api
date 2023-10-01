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
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        private readonly OdinBaselineDbContext _dbContext;

        private DbSet<EmployeeModel> Employees => _dbContext.Set<EmployeeModel>();
        private DbSet<EmployeePositionHistoryModel> EmployeesPositionsHistory => _dbContext.Set<EmployeePositionHistoryModel>();

        public EmployeeRepository(OdinBaselineDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITenantService tenantService)
            : base(httpContextAccessor, tenantService)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee> InsertAsync(Employee employee, CancellationToken cancellationToken)
        {
            var model = employee.ToEmployeeModel(GetTenantId());
            model.SetAuditLog(GetCurrentUsername(), created: true);

            var employeeInserted = await Employees.AddAsync(model, cancellationToken);
            employeeInserted.Reference("Department").Load();

            if (employee.HistoricPositions.Any())
            {
                var historicPositionsToInsert = employee.HistoricPositions
                    .Select(positionHistoric => new EmployeePositionHistoryModel 
                    ( 
                        employee.Id, 
                        positionHistoric.PositionId, 
                        positionHistoric.Salary,
                        positionHistoric.StartDate,
                        positionHistoric.FinishDate,                       
                        positionHistoric.IsActual,
                        GetTenantId()
                    ));

                await EmployeesPositionsHistory.AddRangeAsync(historicPositionsToInsert, cancellationToken);
                employeeInserted.Collection(x => x.HistoricPositions).Load();
            }

            return employeeInserted.Entity.ToEmployee();
        }

        public async Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken)
        {
            var model = employee.ToEmployeeModel(GetTenantId());
            model.SetAuditLog(GetCurrentUsername(), created: false);

            var employeeUpdated = await Task.FromResult(Employees.Update(model));
            employeeUpdated.Reference("Department").Load();

            EmployeesPositionsHistory.RemoveRange(EmployeesPositionsHistory.Where(x => x.EmployeeId == employee.Id));
            
            if (employee.HistoricPositions.Any())
            {
                var historicPositionsToInsert = employee.HistoricPositions
                    .Select(positionHistoric => new EmployeePositionHistoryModel
                    (
                        employee.Id,
                        positionHistoric.PositionId,
                        positionHistoric.Salary,
                        positionHistoric.StartDate,
                        positionHistoric.FinishDate,
                        positionHistoric.IsActual,
                        GetTenantId()
                    ));

                await EmployeesPositionsHistory.AddRangeAsync(historicPositionsToInsert, cancellationToken);
                employeeUpdated.Collection(x => x.HistoricPositions).Load();
            }


            return employeeUpdated.Entity.ToEmployee();
        }

        public Task DeleteAsync(Employee employee)
        {
            EmployeesPositionsHistory.RemoveRange(EmployeesPositionsHistory.Where(x => x.EmployeeId == employee.Id));
            Employees.Remove(employee.ToEmployeeModel(GetTenantId()));

            return Task.CompletedTask;
        }

        public async Task<Employee> FindByIdAsync(Guid id, CancellationToken cancellationToken) 
        {
            var model = await Employees.Include(x => x.Department).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            NotFoundException.ThrowIfNull(model, $"Employee with Id '{id}' not found.");

            var employee = model!.ToEmployee();

            var historicPositions = (await EmployeesPositionsHistory
                .Where(x => x.EmployeeId == employee.Id)
                .ToListAsync(cancellationToken))
                .ToEmployeePositionHistory();

            historicPositions.ForEach(employee.LoadHistoricPosition);
            
            return employee;
        }

        public async Task<PaginatedListOutput<Employee>> FindPaginatedListAsync(Dictionary<string, object?> filters, int pageNumber, int pageSize, string sort, CancellationToken cancellationToken) 
        {
            var filtersExpression = ExpressionsUtility<EmployeeModel>.BuildFilterExpression(filters);
            var expression = ExpressionsUtility<EmployeeModel>.BuildQueryableExpression(filtersExpression);

            var employees = expression != null
                ? await Employees.Where(expression).Include(x => x.Department).ToListAsync(cancellationToken)
                : await Employees.Include(x => x.Department).ToListAsync(cancellationToken);

            var sortedEmployees = SortUtility.ApplySort(employees, sort)!.ToEmployee();

            var employeesIds = sortedEmployees.Select(employee => employee.Id).ToList();
            
            var relations = await EmployeesPositionsHistory.Where(relation => employeesIds.Contains(relation.EmployeeId)).ToListAsync(cancellationToken);
            var relationsByEmployeeIdGroup =relations.GroupBy(x => x.EmployeeId).ToList();

            relationsByEmployeeIdGroup.ForEach(relationGroup => 
            {
                var employee = sortedEmployees.FirstOrDefault(employee => employee.Id == relationGroup.Key);
                
                if (employee is null) 
                    return;
                
                relationGroup.ToList().ForEach(relation => employee.AddHistoricPosition(relation.ToEmployeePositionHistory()));
            });

            return new PaginatedListOutput<Employee>
            (
                totalItems: sortedEmployees.Count(),
                items: sortedEmployees
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
            );
        }

        public async Task<Employee> FindByDocumentAsync(string document, CancellationToken cancellationToken)
        {
            var model = await Employees.Include(x => x.Department).AsNoTracking().FirstOrDefaultAsync(x => x.Document == document, cancellationToken);
            NotFoundException.ThrowIfNull(model, $"Employee with Document '{document}' not found.");

            var employee = model!.ToEmployee();

            var historicPositions = (await EmployeesPositionsHistory
                .Where(x => x.EmployeeId == employee.Id)
                .ToListAsync(cancellationToken))
                .ToEmployeePositionHistory();

            historicPositions.ForEach(employee.LoadHistoricPosition);

            return employee;
        }
    }
}
