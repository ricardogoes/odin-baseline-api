using AutoMapper;
using Odin.Baseline.Data.Helpers;
using Odin.Baseline.Data.Models;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.CompaniesPositions;

namespace Odin.Baseline.Service
{
    public class CompaniesPositionsService : ICompaniesPositionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompaniesPositionsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CompanyPositionToQuery> InsertAsync(CompanyPositionToInsert positionToInsert, string loggedUsername, CancellationToken cancellationToken)
        {
            if (positionToInsert is null)
                throw new ArgumentNullException(nameof(positionToInsert));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            var position = _mapper.Map<CompanyPosition>(positionToInsert);
            position.IsActive = true;
            position.CreatedBy = loggedUsername;
            position.CreatedAt = DateTime.UtcNow;
            position.LastUpdatedBy = loggedUsername;
            position.LastUpdatedAt = DateTime.UtcNow;

            var positionInserted = _unitOfWork.Repository().Insert<CompanyPosition, CompanyPositionToQuery>(position);

            await _unitOfWork.CommitAsync(cancellationToken);

            return positionInserted;
        }

        public async Task<CompanyPositionToQuery> UpdateAsync(CompanyPositionToUpdate positionToUpdate, string loggedUsername, CancellationToken cancellationToken)
        {
            if (positionToUpdate is null)
                throw new ArgumentNullException(nameof(positionToUpdate));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            var position = _mapper.Map<CompanyPosition>(positionToUpdate);
            position.IsActive = true;
            position.LastUpdatedBy = loggedUsername;
            position.LastUpdatedAt = DateTime.UtcNow;

            var positionUpdated = _unitOfWork.Repository().Update<CompanyPosition, CompanyPositionToQuery>(position);

            await _unitOfWork.CommitAsync(cancellationToken);

            if (positionUpdated is null)
                throw new NotFoundException("CompanyPosition not found");

            return positionUpdated;
        }

        public async Task<CompanyPositionToQuery> ChangeStatusAsync(int positionId, string loggedUsername, CancellationToken cancellationToken)
        {
            if (positionId <= 0)
                throw new ArgumentException("Invalid positionId", nameof(positionId));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            try
            {
                var position = _mapper.Map<CompanyPosition>(await GetByIdAsync(positionId, cancellationToken));
                position.IsActive = !position.IsActive;
                position.LastUpdatedBy = loggedUsername;
                position.LastUpdatedAt = DateTime.UtcNow;

                var positionUpdated = _unitOfWork.Repository().Update<CompanyPosition, CompanyPositionToQuery>(position);

                await _unitOfWork.CommitAsync(cancellationToken);

                return positionUpdated;
            }
            catch (NullReferenceException)
            {
                throw new NotFoundException("CompanyPosition not found");
            }
        }

        public async Task<CompanyPositionToQuery> GetByIdAsync(int positionId, CancellationToken cancellationToken)
        {
            if (positionId <= 0)
                throw new ArgumentException("Invalid positionId", nameof(positionId));

            var position = await _unitOfWork.Repository().GetByIdAsync<CompanyPosition, CompanyPositionToQuery>(positionId, cancellationToken);
            return position;
        }

        public async Task<PagedList<CompanyPositionToQuery>> GetByCustomerAsync(int customerId, CompaniesPositionsQueryModel queryData, CancellationToken cancellationToken)
        {
            var filters = BuildFilterExpression(customerId, queryData);

            var positions = await _unitOfWork.Repository().FindListAsync<CompanyPosition, CompanyPositionToQuery>(
                expression: (filters != null ? ExpressionsHelper<CompanyPosition>.BuildQueryableExpression(filters) : null),
                pageNumber: queryData.PageNumber, pageSize: queryData.PageSize, sort: queryData.Sort,
                cancellationToken: cancellationToken);

            return positions;
        }

        private static List<ExpressionFilter> BuildFilterExpression(int customerId, CompaniesPositionsQueryModel queryData)
        {
            var filters = new List<ExpressionFilter>
            {
                new ExpressionFilter { Field = "CustomerId", Operator = ExpressionOperator.Equal, Value = customerId }
            };

            if (!string.IsNullOrWhiteSpace(queryData.Name))
                filters.Add(new ExpressionFilter { Field = nameof(queryData.Name), Operator = ExpressionOperator.Contains, Value = queryData.Name });

            if (queryData.BaseSalary.HasValue)
                filters.Add(new ExpressionFilter { Field = nameof(queryData.BaseSalary), Operator = ExpressionOperator.Equal, Value = queryData.BaseSalary.Value });

            if (queryData.IsActive.HasValue)
                filters.Add(new ExpressionFilter { Field = nameof(queryData.IsActive), Operator = ExpressionOperator.Equal, Value = queryData.IsActive.Value });

            return filters;
        }
    }
}
