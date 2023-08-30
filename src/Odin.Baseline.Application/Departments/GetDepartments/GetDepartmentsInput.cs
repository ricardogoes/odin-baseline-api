using MediatR;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.DTO.Common;

namespace Odin.Baseline.Application.Departments.GetDepartments
{
    public class GetDepartmentsInput : PaginatedListInput, IRequest<PaginatedListOutput<DepartmentOutput>>
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public GetDepartmentsInput()
        { }

        public GetDepartmentsInput(int page, int pageSize, string sort, Guid customerId, string name, bool? isActive)
            : base(page, pageSize, sort)
        {
            CustomerId = customerId;
            Name = name;
            IsActive = isActive;
        }
    }
}
