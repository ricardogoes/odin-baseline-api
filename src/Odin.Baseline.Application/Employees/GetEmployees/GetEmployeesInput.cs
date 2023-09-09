using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.DTO.Common;

namespace Odin.Baseline.Application.Employees.GetEmployees
{
    public class GetEmployeesInput : PaginatedListInput, IRequest<PaginatedListOutput<EmployeeOutput>>
    {
        public Guid? CustomerId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAtStart { get; set; }
        public DateTime? CreatedAtEnd { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAtStart { get; set; }
        public DateTime? LastUpdatedAtEnd { get; set; }

        public GetEmployeesInput()
            : base()
        { }

        public GetEmployeesInput(int page, int pageSize, string sort, Guid? customerId, Guid? departmentId, string firstName, string lastName, string document, string email, bool? isActive,
            string createdBy, DateTime? createdAtStart, DateTime? createdAtEnd,
            string lastUpdatedBy, DateTime? lastUpdatedAtStart, DateTime? lastUpdatedAtEnd)
            : base(page, pageSize, sort)
        {
            CustomerId = customerId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
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
