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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly OdinBaselineDbContext _dbContext;

        private DbSet<CustomerModel> _customers => _dbContext.Set<CustomerModel>();

        public CustomerRepository(OdinBaselineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> InsertAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _customers.AddAsync(customer.ToCustomerModel(), cancellationToken);

            return customer;

        }

        public async Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken)
        {
            await Task.FromResult(_customers.Update(customer.ToCustomerModel()));
            return customer;
        }

        public async Task DeleteAsync(Customer customer)
            => await Task.FromResult(_customers.Remove(customer.ToCustomerModel()));

        public async Task<Customer> FindByIdAsync(Guid id, CancellationToken cancellationToken) 
        {
            var model = await _customers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            NotFoundException.ThrowIfNull(model, $"Customer with Id '{id}' not found.");

            return model.ToCustomer(); 
        }

        public async Task<PaginatedListOutput<Customer>> FindPaginatedListAsync(Dictionary<string, object> filters, int pageNumber, int pageSize, string sort, CancellationToken cancellationToken) 
        {
            var filtersExpression = ExpressionsFactory<CustomerModel>.BuildFilterExpression(filters);
            var expression = ExpressionsFactory<CustomerModel>.BuildQueryableExpression(filtersExpression);

            var data = expression != null ? await _customers.AsNoTracking().Where(expression).ToListAsync(cancellationToken) : await _customers.ToListAsync(cancellationToken);

            var sortedData = SortHelper.ApplySort<CustomerModel>(data, sort);

            return new PaginatedListOutput<Customer>
            {
                TotalItems = sortedData.Count(),
                Items = sortedData
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToCustomer()
            };
        }

        public async Task<Customer> FindByDocumentAsync(string document, CancellationToken cancellationToken)
        {
            var model = await _customers.AsNoTracking().SingleOrDefaultAsync(x => x.Document == document, cancellationToken);
            NotFoundException.ThrowIfNull(model, $"Customer with Document '{document}' not found.");

            return model.ToCustomer();
        }

        /*public async Task<IEnumerable<Guid>> FindListIdsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
            => await _customers.AsNoTracking().Where(entity => ids.Contains(entity.Id)).Select(entity => entity.Id).ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Customer>> FindListByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
        {
            var models = await _customers.AsNoTracking().Where(entity => ids.Contains(entity.Id)).ToListAsync(cancellationToken);
            return models.ToCustomer().ToList().AsReadOnly();
        }*/
    }
}
