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

        private DbSet<EmployeeModel> _employees => _dbContext.Set<EmployeeModel>();
        private DbSet<EmployeePositionHistoryModel> _employeesPositionsHistory => _dbContext.Set<EmployeePositionHistoryModel>();

        public EmployeeRepository(OdinBaselineDbContext dbContext)
            => _dbContext = dbContext;

        public async Task InsertAsync(Employee employee, CancellationToken cancellationToken)
        {
            await _employees.AddAsync(employee.ToEmployeeModel(), cancellationToken);

            if (employee.HistoricPositions.Any())
            {
                var historicPositions = employee.HistoricPositions
                    .Select(positionHistoric => new EmployeePositionHistoryModel 
                    { 
                        EmployeeId = employee.Id, 
                        PositionId = positionHistoric.PositionId, 
                        Salary = positionHistoric.Salary,
                        StartDate = positionHistoric.StartDate,
                        FinishDate = positionHistoric.FinishDate,                       
                        IsActual = positionHistoric.IsActual
                    });

                await _employeesPositionsHistory.AddRangeAsync(historicPositions);
            }
        }

        public async Task UpdateAsync(Employee employee)
        {
            _employees.Update(employee.ToEmployeeModel());
            _employeesPositionsHistory.RemoveRange(_employeesPositionsHistory.Where(x => x.EmployeeId == employee.Id));
            
            if (employee.HistoricPositions.Any())
            {
                var historicPositions = employee.HistoricPositions
                    .Select(positionHistoric => new EmployeePositionHistoryModel
                    {
                        EmployeeId = employee.Id,
                        PositionId = positionHistoric.PositionId,
                        Salary = positionHistoric.Salary,
                        StartDate = positionHistoric.StartDate,
                        FinishDate = positionHistoric.FinishDate,
                        IsActual = positionHistoric.IsActual
                    });

                await _employeesPositionsHistory.AddRangeAsync(historicPositions);
            }
        }

        public Task DeleteAsync(Employee employee)
        {
            _employeesPositionsHistory.RemoveRange(_employeesPositionsHistory.Where(x => x.EmployeeId == employee.Id));
            _employees.Remove(employee.ToEmployeeModel());

            return Task.CompletedTask;
        }

        public async Task<Employee> FindByIdAsync(Guid id, CancellationToken cancellationToken) 
        {
            var model = await _employees.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
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

            var employees = expression != null ? await _employees.AsNoTracking().Where(expression).ToListAsync(cancellationToken) : await _employees.ToListAsync(cancellationToken);

            var sortedEmployees = SortHelper.ApplySort(employees, sort);

            var employeesIds = sortedEmployees.Select(employee => employee.Id).ToList();
            
            var relations = await _employeesPositionsHistory.Where(relation => employeesIds.Contains(relation.EmployeeId)).ToListAsync();
            var relationsByEmployeeIdGroup =relations.GroupBy(x => x.EmployeeId).ToList();

            relationsByEmployeeIdGroup.ForEach(relationGroup => 
            {
                var employeeModel = sortedEmployees.FirstOrDefault(employee => employee.Id == relationGroup.Key);
                var employee = employeeModel.ToEmployee();
                
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
                    .ToEmployee()
            };
        }

        public async Task<Employee> FindByDocumentAsync(string document, CancellationToken cancellationToken)
        {
            var model = await _employees.AsNoTracking().FirstOrDefaultAsync(x => x.Document == document, cancellationToken);
            NotFoundException.ThrowIfNull(model, $"Employee with Document '{document}' not found.");

            return model.ToEmployee();
        }
    }
}
