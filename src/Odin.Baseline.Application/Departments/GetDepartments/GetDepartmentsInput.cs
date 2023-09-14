using MediatR;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.DTO.Common;

namespace Odin.Baseline.Application.Departments.GetDepartments
{
    public class GetDepartmentsInput : PaginatedListInput, IRequest<PaginatedListOutput<DepartmentOutput>>
    {
        public Guid CustomerId { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAtStart { get; set; }
        public DateTime? CreatedAtEnd { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAtStart { get; set; }
        public DateTime? LastUpdatedAtEnd { get; set; }

        public GetDepartmentsInput()
            : base()
        { }

        public GetDepartmentsInput(int page, int pageSize, Guid customerId, string? sort = null, string? name = null, bool? isActive = null, 
            string? createdBy = null, DateTime? createdAtStart = null, DateTime? createdAtEnd = null, 
            string? lastUpdatedBy = null, DateTime? lastUpdatedAtStart = null, DateTime? lastUpdatedAtEnd = null)
            : base(page, pageSize, sort)
        {
            CustomerId = customerId;
            Name = name;
            IsActive = isActive;

            CreatedBy = createdBy;
            CreatedAtStart = createdAtStart;
            CreatedAtEnd = createdAtEnd;

            LastUpdatedBy = lastUpdatedBy;
            LastUpdatedAtStart = lastUpdatedAtStart;
            LastUpdatedAtEnd = lastUpdatedAtEnd;
        }
    }
}
