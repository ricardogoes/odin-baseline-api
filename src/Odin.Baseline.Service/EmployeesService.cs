using AutoMapper;
using Odin.Baseline.Data.Helpers;
using Odin.Baseline.Data.Models;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Employees;

namespace Odin.Baseline.Service
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<EmployeeToQuery> InsertAsync(EmployeeToInsert employeeToInsert, string loggedUsername, CancellationToken cancellationToken)
        {
            if (employeeToInsert is null)
                throw new ArgumentNullException(nameof(employeeToInsert));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            var employee = _mapper.Map<Employee>(employeeToInsert);
            employee.IsActive = true;
            employee.CreatedBy = loggedUsername;
            employee.CreatedAt = DateTime.UtcNow;
            employee.LastUpdatedBy = loggedUsername;
            employee.LastUpdatedAt = DateTime.UtcNow;

            var employeeInserted = _unitOfWork.Repository().Insert<Employee, EmployeeToQuery>(employee);

            await _unitOfWork.CommitAsync(cancellationToken);

            return employeeInserted;
        }

        public async Task<EmployeeToQuery> UpdateAsync(EmployeeToUpdate employeeToUpdate, string loggedUsername, CancellationToken cancellationToken)
        {
            if (employeeToUpdate is null)
                throw new ArgumentNullException(nameof(employeeToUpdate));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            var employee = _mapper.Map<Employee>(employeeToUpdate);
            employee.IsActive = true;
            employee.LastUpdatedBy = loggedUsername;
            employee.LastUpdatedAt = DateTime.UtcNow;

            var employeeUpdated = _unitOfWork.Repository().Update<Employee, EmployeeToQuery>(employee);

            await _unitOfWork.CommitAsync(cancellationToken);

            if (employeeUpdated is null)
                throw new NotFoundException("Employee not found");

            return employeeUpdated;
        }

        public async Task<EmployeeToQuery> ChangeStatusAsync(int employeeId, string loggedUsername, CancellationToken cancellationToken)
        {
            if (employeeId <= 0)
                throw new ArgumentException("Invalid employeeId", nameof(employeeId));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            try
            {
                var employee = _mapper.Map<Employee>(await GetByIdAsync(employeeId, cancellationToken));
                employee.IsActive = !employee.IsActive;
                employee.LastUpdatedBy = loggedUsername;
                employee.LastUpdatedAt = DateTime.UtcNow;

                var employeeUpdated = _unitOfWork.Repository().Update<Employee, EmployeeToQuery>(employee);

                await _unitOfWork.CommitAsync(cancellationToken);

                return employeeUpdated;
            }
            catch (NullReferenceException)
            {
                throw new NotFoundException("Employee not found");
            }
        }

        public async Task<EmployeeToQuery> GetByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            if (employeeId <= 0)
                throw new ArgumentException("Invalid employeeId", nameof(employeeId));

            var employee = await _unitOfWork.Repository().GetByIdAsync<Employee, EmployeeToQuery>(employeeId, cancellationToken);
            return employee;
        }

        public async Task<PagedList<EmployeeToQuery>> GetByCustomerAsync(int customerId, EmployeesQueryModel queryData, CancellationToken cancellationToken)
        {
            var filters = BuildFilterExpression(
                customerId: customerId,
                departmentId: null,
                positionId: null,
                queryData);

            var employees = await _unitOfWork.Repository().FindListAsync<Employee, EmployeeToQuery>(
                expression: (filters != null ? ExpressionsHelper<Employee>.BuildQueryableExpression(filters) : null),
                pageNumber: queryData.PageNumber, pageSize: queryData.PageSize, sort: queryData.Sort,
                cancellationToken: cancellationToken);

            return employees;
        }

        public async Task<PagedList<EmployeeToQuery>> GetByDepartmentAsync(int departmentId, EmployeesQueryModel queryData, CancellationToken cancellationToken)
        {
            var filters = BuildFilterExpression(
                customerId: null,
                departmentId: departmentId,
                positionId: null,
                queryData);

            var employees = await _unitOfWork.Repository().FindListAsync<Employee, EmployeeToQuery>(
                expression: (filters != null ? ExpressionsHelper<Employee>.BuildQueryableExpression(filters) : null),
                pageNumber: queryData.PageNumber, pageSize: queryData.PageSize, sort: queryData.Sort,
                cancellationToken: cancellationToken);

            return employees;
        }

        public async Task<PagedList<EmployeeToQuery>> GetByCompanyPositionAsync(int positionId, EmployeesQueryModel queryData, CancellationToken cancellationToken)
        {
            var filters = BuildFilterExpression(
                customerId: null,
                departmentId: null,
                positionId: positionId,
                queryData);

            var employees = await _unitOfWork.Repository().FindListAsync<Employee, EmployeeToQuery>(
                expression: (filters != null ? ExpressionsHelper<Employee>.BuildQueryableExpression(filters) : null),
                pageNumber: queryData.PageNumber, pageSize: queryData.PageSize, sort: queryData.Sort,
                cancellationToken: cancellationToken);

            return employees;
        }

        private static List<ExpressionFilter> BuildFilterExpression(int? customerId, int? departmentId, int? positionId, EmployeesQueryModel queryData)
        {
            var filters = new List<ExpressionFilter>();

            if (customerId.HasValue)
                filters.Add(new ExpressionFilter { Field = "CustomerId", Operator = ExpressionOperator.Equal, Value = customerId });

            if (departmentId.HasValue)
                filters.Add(new ExpressionFilter { Field = "DepartmentId", Operator = ExpressionOperator.Equal, Value = departmentId });

            if (positionId.HasValue)
                filters.Add(new ExpressionFilter { Field = "CompanyPositionId", Operator = ExpressionOperator.Equal, Value = positionId });

            if (!string.IsNullOrWhiteSpace(queryData.FirstName))
                filters.Add(new ExpressionFilter { Field = nameof(queryData.FirstName), Operator = ExpressionOperator.Contains, Value = queryData.FirstName });

            if (!string.IsNullOrWhiteSpace(queryData.LastName))
                filters.Add(new ExpressionFilter { Field = nameof(queryData.LastName), Operator = ExpressionOperator.Contains, Value = queryData.LastName });

            if (!string.IsNullOrWhiteSpace(queryData.Email))
                filters.Add(new ExpressionFilter { Field = nameof(queryData.Email), Operator = ExpressionOperator.Contains, Value = queryData.Email });

            if (queryData.Salary.HasValue)
                filters.Add(new ExpressionFilter { Field = nameof(queryData.Salary), Operator = ExpressionOperator.Equal, Value = queryData.Salary.Value });

            if (queryData.IsActive.HasValue)
                filters.Add(new ExpressionFilter { Field = nameof(queryData.IsActive), Operator = ExpressionOperator.Equal, Value = queryData.IsActive.Value });

            return filters;
        }
    }
}
