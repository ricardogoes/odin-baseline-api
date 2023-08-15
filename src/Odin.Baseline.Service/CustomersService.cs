using AutoMapper;
using Odin.Baseline.Data.Helpers;
using Odin.Baseline.Data.Models;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Customers;

namespace Odin.Baseline.Service
{
    public class CustomersService : ICustomersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Customer> InsertAsync(CustomerToInsert customerToInsert, string loggedUsername, CancellationToken cancellationToken)
        {
            if (customerToInsert is null)
                throw new ArgumentNullException(nameof(customerToInsert));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            var customer = _mapper.Map<Customer>(customerToInsert);
            customer.IsActive = true;
            customer.CreatedBy = loggedUsername;
            customer.CreatedAt = DateTime.UtcNow;
            customer.LastUpdatedBy = loggedUsername;
            customer.LastUpdatedAt = DateTime.UtcNow;

            var customerInserted = _unitOfWork.Repository().Insert<Customer, Customer>(customer);

            await _unitOfWork.CommitAsync(cancellationToken);

            return customerInserted;
        }

        public async Task<Customer> UpdateAsync(CustomerToUpdate customerToUpdate, string loggedUsername, CancellationToken cancellationToken)
        {
            if (customerToUpdate is null)
                throw new ArgumentNullException(nameof(customerToUpdate));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            var customer = _mapper.Map<Customer>(customerToUpdate);
            customer.IsActive = true;
            customer.LastUpdatedBy = loggedUsername;
            customer.LastUpdatedAt = DateTime.UtcNow;

            var customerUpdated = _unitOfWork.Repository().Update<Customer, Customer>(customer);
            await _unitOfWork.CommitAsync(cancellationToken);

            if (customerUpdated is null)
                throw new NotFoundException("Customer not found");

            return customerUpdated;
        }

        public async Task<Customer> ChangeStatusAsync(int customerId, string loggedUsername, CancellationToken cancellationToken)
        {
            if (customerId <= 0)
                throw new ArgumentException("Invalid customerId", nameof(customerId));

            if (string.IsNullOrWhiteSpace(loggedUsername))
                throw new ArgumentNullException(nameof(loggedUsername));

            try
            {
                var customer = await GetByIdAsync(customerId, cancellationToken);                
                customer.IsActive = !customer.IsActive;
                customer.LastUpdatedBy = loggedUsername;
                customer.LastUpdatedAt = DateTime.UtcNow;

                var customerUpdated = _unitOfWork.Repository().Update<Customer, Customer>(customer);

                await _unitOfWork.CommitAsync(cancellationToken);

                return customerUpdated;
            }
            catch (NullReferenceException)
            {
                throw new NotFoundException("Customer not found");
            }            
        }

        public async Task<PagedList<Customer>> GetAllAsync(CustomersQueryModel queryData, CancellationToken cancellationToken)
        {
            var filters = BuildFilterExpression(queryData);

            var departments = await _unitOfWork.Repository().FindListAsync<Customer, Customer>(
                expression: (filters != null ? ExpressionsHelper<Customer>.BuildQueryableExpression(filters) : null),
                pageNumber: queryData.PageNumber, pageSize: queryData.PageSize, sort: queryData.Sort,
                cancellationToken: cancellationToken);

            return departments;
        }

        public async Task<Customer> GetByIdAsync(int customerId, CancellationToken cancellationToken)
        {
            if (customerId <= 0)
                throw new ArgumentException("Invalid customerId", nameof(customerId));

            return await _unitOfWork.Repository().GetByIdAsync<Customer, Customer>(customerId, cancellationToken);
        }

        private static List<ExpressionFilter> BuildFilterExpression(CustomersQueryModel queryData)
        {
            var filters = new List<ExpressionFilter>();

            if (!string.IsNullOrWhiteSpace(queryData.Name))
                filters.Add(new ExpressionFilter { Field = nameof(queryData.Name), Operator = ExpressionOperator.Contains, Value = queryData.Name });

            if (!string.IsNullOrWhiteSpace(queryData.Document))
                filters.Add(new ExpressionFilter { Field = nameof(queryData.Document), Operator = ExpressionOperator.Contains, Value = queryData.Document });

            if (queryData.IsActive.HasValue)
                filters.Add(new ExpressionFilter { Field = nameof(queryData.IsActive), Operator = ExpressionOperator.Equal, Value = queryData.IsActive.Value });

            return filters.Any() ? filters : null;
        }
    }
}
