using MediatR;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.DTO.Common;

namespace Odin.Baseline.Application.Customers.GetCustomers
{
    public class GetCustomersInput : PaginatedListInput, IRequest<PaginatedListOutput<CustomerOutput>>
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public bool? IsActive { get; set; }

        public GetCustomersInput()
        { }

        public GetCustomersInput(int page, int pageSize, string sort, string name, string document, bool? isActive)
            : base(page, pageSize, sort)
        {
            Name = name;
            Document = document;
            IsActive = isActive;
        }
    }
}
