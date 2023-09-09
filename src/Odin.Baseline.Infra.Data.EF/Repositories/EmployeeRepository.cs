using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Infra.Data.EF.Expressions;
using Odin.Baseline.Infra.Data.EF.Helpers;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly OdinBaselineDbContext _dbContext;

        private DbSet<DepartmentModel> _departments => _dbContext.Set<DepartmentModel>();
        private DbSet<EmployeeModel> _employees => _dbContext.Set<EmployeeModel>();
        private DbSet<EmployeePositionHistoryModel> _employeesPositionsHistory => _dbContext.Set<EmployeePositionHistoryModel>();

        public EmployeeRepository(OdinBaselineDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<Employee> InsertAsync(Employee employee, CancellationToken cancellationToken)
        {
            var employeeInserted = await _employees.AddAsync(employee.ToEmployeeModel(), cancellationToken);
            employeeInserted.Reference("Customer").Load();
            employeeInserted.Reference("Department").Load();

            if (employee.HistoricPositions.Any())
            {
                var historicPositionsToInsert = employee.HistoricPositions
                    .Select(positionHistoric => new EmployeePositionHistoryModel 
                    { 
                        EmployeeId = employee.Id, 
                        PositionId = positionHistoric.PositionId, 
                        Salary = positionHistoric.Salary,
                        StartDate = positionHistoric.StartDate,
                        FinishDate = positionHistoric.FinishDate,                       
                        IsActual = positionHistoric.IsActual
                    });

                await _employeesPositionsHistory.AddRangeAsync(historicPositionsToInsert);
                employeeInserted.Collection(x => x.HistoricPositions).Load();
            }

            return employeeInserted.Entity.ToEmployee();
        }

        public async Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken)
        {
            var employeeUpdated = _employees.Update(employee.ToEmployeeModel());

            employeeUpdated.Reference("Customer").Load();
            employeeUpdated.Reference("Department").Load();

            _employeesPositionsHistory.RemoveRange(_employeesPositionsHistory.Where(x => x.EmployeeId == employee.Id));
            
            if (employee.HistoricPositions.Any())
            {
                var historicPositionsToInsert = employee.HistoricPositions
                    .Select(positionHistoric => new EmployeePositionHistoryModel
                    {
                        EmployeeId = employee.Id,
                        PositionId = positionHistoric.PositionId,
                        Salary = positionHistoric.Salary,
                        StartDate = positionHistoric.StartDate,
                        FinishDate = positionHistoric.FinishDate,
                        IsActual = positionHistoric.IsActual
                    });

                await _employeesPositionsHistory.AddRangeAsync(historicPositionsToInsert);
                employeeUpdated.Collection(x => x.HistoricPositions).Load();
            }


            return employeeUpdated.Entity.ToEmployee();
        }

        public Task DeleteAsync(Employee employee)
        {
            _employeesPositionsHistory.RemoveRange(_employeesPositionsHistory.Where(x => x.EmployeeId == employee.Id));
            _employees.Remove(employee.ToEmployeeModel());

            return Task.CompletedTask;
        }

        public async Task<Employee> FindByIdAsync(Guid id, CancellationToken cancellationToken) 
        {
            var model = await _employees.Include(x => x.Customer).Include(x => x.Department).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            NotFoundException.ThrowIfNull(model, $"Employee with Id '{id}' not found.");

            var employee = model.ToEmployee();

            var historicPositions = (await _employeesPositionsHistory
                .Where(x => x.EmployeeId == employee.Id)
                .ToListAsync(cancellationToken))
                .ToEmployeePositionHistory();

            historicPositions.ForEach(employee.LoadHistoricPosition);
            
            return employee;
        }

        public async Task<PaginatedListOutput<Employee>> FindPaginatedListAsync(Dictionary<string, object> filters, int pageNumber, int pageSize, string sort, CancellationToken cancellationToken) 
        {
            var filtersExpression = ExpressionsFactory<EmployeeModel>.BuildFilterExpression(filters);
            var expression = ExpressionsFactory<EmployeeModel>.BuildQueryableExpression(filtersExpression);

            var employees = expression != null
                ? await _employees.Where(expression).Include(x => x.Customer).Include(x => x.Department).ToListAsync(cancellationToken)
                : await _employees.Include(x => x.Customer).Include(x => x.Department).ToListAsync(cancellationToken);

            var sortedEmployees = SortHelper.ApplySort(employees, sort).ToEmployee();

            var employeesIds = sortedEmployees.Select(employee => employee.Id).ToList();
            
            var relations = await _employeesPositionsHistory.Where(relation => employeesIds.Contains(relation.EmployeeId)).ToListAsync();
            var relationsByEmployeeIdGroup =relations.GroupBy(x => x.EmployeeId).ToList();

            relationsByEmployeeIdGroup.ForEach(relationGroup => 
            {
                var employee = sortedEmployees.FirstOrDefault(employee => employee.Id == relationGroup.Key);
                
                if (employee is null) 
                    return;
                
                relationGroup.ToList().ForEach(relation => employee.AddHistoricPosition(relation.ToEmployeePositionHistory()));
            });

            return new PaginatedListOutput<Employee>
            {
                TotalItems = sortedEmployees.Count(),
                Items = sortedEmployees
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
            };
        }

        public async Task<Employee> FindByDocumentAsync(string document, CancellationToken cancellationToken)
        {
            var model = await _employees.Include(x => x.Customer).Include(x => x.Department).AsNoTracking().FirstOrDefaultAsync(x => x.Document == document, cancellationToken);
            NotFoundException.ThrowIfNull(model, $"Employee with Document '{document}' not found.");

            var employee = model.ToEmployee();

            var historicPositions = (await _employeesPositionsHistory
                .Where(x => x.EmployeeId == employee.Id)
                .ToListAsync(cancellationToken))
                .ToEmployeePositionHistory();

            historicPositions.ForEach(employee.LoadHistoricPosition);

            return employee;
        }
    }
}
