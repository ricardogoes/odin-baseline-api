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

        public GetEmployeesInput()
        { }

        public GetEmployeesInput(int page, int pageSize, string sort, Guid? customerId, Guid? departmentId, string firstName, string lastName, string document, string email, bool? isActive)
            : base(page, pageSize, sort)
        {
            CustomerId = customerId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
            IsActive = isActive;
        }
    }
}
