using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Models;
using Odin.Infra.Data.Utilities.Expressions;
using Odin.Infra.Data.Utilities.Sort;

namespace Odin.Baseline.Infra.Data.EF.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly OdinBaselineDbContext _dbContext;

        private DbSet<CustomerModel> Customers => _dbContext.Set<CustomerModel>();

        public CustomerRepository(OdinBaselineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> InsertAsync(Customer customer, CancellationToken cancellationToken)
        {
            await Customers.AddAsync(customer.ToCustomerModel(), cancellationToken);

            return customer;

        }

        public async Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken)
        {
            await Task.FromResult(Customers.Update(customer.ToCustomerModel()));
            return customer;
        }

        public async Task DeleteAsync(Customer customer)
            => await Task.FromResult(Customers.Remove(customer.ToCustomerModel()));

        public async Task<Customer> FindByIdAsync(Guid id, CancellationToken cancellationToken) 
        {
            var model = await Customers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            NotFoundException.ThrowIfNull(model, $"Customer with Id '{id}' not found.");

            return model!.ToCustomer(); 
        }

        public async Task<PaginatedListOutput<Customer>> FindPaginatedListAsync(Dictionary<string, object?> filters, int pageNumber, int pageSize, string sort, CancellationToken cancellationToken) 
        {
            var filtersExpression = ExpressionsUtility<CustomerModel>.BuildFilterExpression(filters);
            var expression = ExpressionsUtility<CustomerModel>.BuildQueryableExpression(filtersExpression);

            var data = expression != null ? await Customers.AsNoTracking().Where(expression).ToListAsync(cancellationToken) : await Customers.ToListAsync(cancellationToken);

            var sortedData = SortUtility.ApplySort(data, sort)!;

            return new PaginatedListOutput<Customer>
            (
                totalItems: sortedData.Count(),
                items: sortedData
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToCustomer()
            );
        }

        public async Task<Customer> FindByDocumentAsync(string document, CancellationToken cancellationToken)
        {
            var model = await Customers.AsNoTracking().SingleOrDefaultAsync(x => x.Document == document, cancellationToken);
            NotFoundException.ThrowIfNull(model, $"Customer with Document '{document}' not found.");

            return model!.ToCustomer();
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
