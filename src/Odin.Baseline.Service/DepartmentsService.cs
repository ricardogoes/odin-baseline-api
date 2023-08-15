using AutoMapper;
using Odin.Baseline.Data.Helpers;
using Odin.Baseline.Data.Models;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Departments;

namespace Odin.Baseline.Service
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DepartmentToQuery> InsertAsync(DepartmentToInsert departmentToInsert, string loggedUsername, CancellationToken cancellationToken)
        {
            if (departmentToInsert is null)
                throw new ArgumentNullException(nameof(departmentToInsert));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            var department = _mapper.Map<Department>(departmentToInsert);
            department.IsActive = true;
            department.CreatedBy = loggedUsername;
            department.CreatedAt = DateTime.UtcNow;
            department.LastUpdatedBy = loggedUsername;
            department.LastUpdatedAt = DateTime.UtcNow;

            var departmentInserted = _unitOfWork.Repository().Insert<Department, DepartmentToQuery>(department);

            await _unitOfWork.CommitAsync(cancellationToken);

            return departmentInserted;
        }

        public async Task<DepartmentToQuery> UpdateAsync(DepartmentToUpdate departmentToUpdate, string loggedUsername, CancellationToken cancellationToken)
        {
            if (departmentToUpdate is null)
                throw new ArgumentNullException(nameof(departmentToUpdate));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            var department = _mapper.Map<Department>(departmentToUpdate);
            department.IsActive = true;
            department.LastUpdatedBy = loggedUsername;
            department.LastUpdatedAt = DateTime.UtcNow;

            var departmentUpdated = _unitOfWork.Repository().Update<Department, DepartmentToQuery>(department);

            await _unitOfWork.CommitAsync(cancellationToken);

            if (departmentUpdated is null)
                throw new NotFoundException("Department not found");

            return departmentUpdated;
        }

        public async Task<DepartmentToQuery> ChangeStatusAsync(int departmentId, string loggedUsername, CancellationToken cancellationToken)
        {
            if (departmentId <= 0)
                throw new ArgumentException("Invalid departmentId", nameof(departmentId));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            try
            {
                var department = _mapper.Map<Department>(await GetByIdAsync(departmentId, cancellationToken));
                department.IsActive = !department.IsActive;
                department.LastUpdatedBy = loggedUsername;
                department.LastUpdatedAt = DateTime.UtcNow;

                var departmentUpdated = _unitOfWork.Repository().Update<Department, DepartmentToQuery>(department);

                await _unitOfWork.CommitAsync(cancellationToken);

                return departmentUpdated;
            }
            catch (NullReferenceException)
            {
                throw new NotFoundException("Department not found");
            }
        }

        public async Task<DepartmentToQuery> GetByIdAsync(int departmentId, CancellationToken cancellationToken)
        {
            if (departmentId <= 0)
                throw new ArgumentException("Invalid departmentId", nameof(departmentId));

            var department = await _unitOfWork.Repository().GetByIdAsync<Department, DepartmentToQuery>(departmentId, cancellationToken);
            return _mapper.Map<DepartmentToQuery>(department);
        }

        public async Task<PagedList<DepartmentToQuery>> GetByCustomerAsync(int customerId, DepartmentsQueryModel queryData, CancellationToken cancellationToken)
        {
            var filters = BuildFilterExpression(customerId, queryData);

            var departments = await _unitOfWork.Repository().FindListAsync<Department, DepartmentToQuery>(
                expression: (filters != null ? ExpressionsHelper<Department>.BuildQueryableExpression(filters) : null),
                pageNumber: queryData.PageNumber, pageSize: queryData.PageSize, sort: queryData.Sort,
                cancellationToken: cancellationToken);

            return departments;
        }

        private static List<ExpressionFilter> BuildFilterExpression(int customerId, DepartmentsQueryModel queryData)
        {
            var filters = new List<ExpressionFilter>
            {
                new ExpressionFilter { Field = "CustomerId", Operator = ExpressionOperator.Equal, Value = customerId }
            };

            if (!string.IsNullOrWhiteSpace(queryData.Name))
                filters.Add(new ExpressionFilter { Field = nameof(queryData.Name), Operator = ExpressionOperator.Contains, Value = queryData.Name });

            if (queryData.IsActive.HasValue)
                filters.Add(new ExpressionFilter { Field = nameof(queryData.IsActive), Operator = ExpressionOperator.Equal, Value = queryData.IsActive.Value });

            return filters;
        }
    }
}
