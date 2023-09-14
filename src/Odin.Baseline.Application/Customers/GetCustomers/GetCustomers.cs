using MediatR;
using Odin.Baseline.Application.Common;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Customers.GetCustomers
{
    public class GetCustomers : IRequestHandler<GetCustomersInput, PaginatedListOutput<CustomerOutput>>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomers(ICustomerRepository repository)
            => _repository = repository;

        public async Task<PaginatedListOutput<CustomerOutput>> Handle(GetCustomersInput input, CancellationToken cancellationToken)
        {
            var filters = new Dictionary<string, object?>
            {
                { "Name", input.Name },
                { "Document", input.Document },
                { "IsActive", input.IsActive },
            };

            var customers = await _repository.FindPaginatedListAsync(
                filters, input.PageNumber, input.PageSize, input.Sort!,
                cancellationToken: cancellationToken);

            return new PaginatedListOutput<CustomerOutput>
            (
                pageNumber: input.PageNumber,
                pageSize: input.PageSize,
                totalPages: PaginatedListHelper.GetTotalPages(customers.TotalItems, input.PageSize),
                totalItems: customers.TotalItems,
                items: CustomerOutput.FromCustomer(customers.Items)
            );
        }
    }
}
