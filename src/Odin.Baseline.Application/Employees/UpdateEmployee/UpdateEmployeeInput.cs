using MediatR;
using Odin.Baseline.Application.Employees.Common;

namespace Odin.Baseline.Application.Employees.UpdateEmployee
{
    public class UpdateEmployeeInput : IRequest<EmployeeOutput>
    {       
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid? DepartmentId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Document { get; private set; }
        public string Email { get; private set; }
        public string LoggedUsername { get; private set; }

        public UpdateEmployeeInput(Guid id, Guid customerId, string firstName, string lastName, string document, string email, string loggedUsername, Guid? departmentId = null)
        {
            Id = id;
            CustomerId = customerId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
            LoggedUsername = loggedUsername;
        }

        public void ChangeId(Guid id)
        {
            Id = id;
        }

        public void ChangeDocument(string document)
        {
            Document = document;
        }

        public void ChangeLoggedUsername(string username)
        {
            LoggedUsername = username;
        }
    }
}
