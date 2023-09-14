using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.DTO.Common;

namespace Odin.Baseline.Application.Employees.GetEmployees
{
    public class GetEmployeesInput : PaginatedListInput, IRequest<PaginatedListOutput<EmployeeOutput>>
    {
        public Guid? CustomerId { get; private set; }
        public Guid? DepartmentId { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Document { get; private set; }
        public string? Email { get; private set; }
        public bool? IsActive { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime? CreatedAtStart { get; private set; }
        public DateTime? CreatedAtEnd { get; private set; }
        public string? LastUpdatedBy { get; private set; }
        public DateTime? LastUpdatedAtStart { get; private set; }
        public DateTime? LastUpdatedAtEnd { get; private set; }

        public GetEmployeesInput()
            : base()
        { }

        public GetEmployeesInput(Guid? customerId, Guid? departmentId, string firstName, string lastName, string document, 
            string email, bool? isActive, string createdBy, DateTime? createdAtStart, DateTime? createdAtEnd, 
            string lastUpdatedBy, DateTime? lastUpdatedAtStart, DateTime? lastUpdatedAtEnd)
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

        public GetEmployeesInput(int page, int pageSize, Guid? customerId = null, string? sort = null, Guid? departmentId = null, string? firstName = null, string? lastName = null, 
            string? document = null, string? email = null, bool? isActive = null,
            string? createdBy = null, DateTime? createdAtStart = null, DateTime? createdAtEnd = null,
            string? lastUpdatedBy = null, DateTime? lastUpdatedAtStart = null, DateTime? lastUpdatedAtEnd = null)
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
