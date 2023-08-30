using MediatR;
using Odin.Baseline.Application.Employees.Common;

namespace Odin.Baseline.Application.Employees.UpdateEmployee
{
    public class UpdateEmployeeInput : IRequest<EmployeeOutput>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string LoggedUsername { get; set; }
    }
}
