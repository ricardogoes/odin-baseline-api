using MediatR;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.DTO.Common;

namespace Odin.Baseline.Application.Customers.GetCustomers
{
    public class GetCustomersInput : PaginatedListInput, IRequest<PaginatedListOutput<CustomerOutput>>
    {
        public string? Name { get; private set; }
        public string? Document { get; private set; }
        public bool? IsActive { get; private set; }

        public GetCustomersInput()
        { }

        public GetCustomersInput(int page, int pageSize, string? sort = null, string? name = null, string? document = null, bool? isActive = null)
            : base(page, pageSize, sort)
        {
            Name = name;
            Document = document;
            IsActive = isActive;
        }
    }
}
