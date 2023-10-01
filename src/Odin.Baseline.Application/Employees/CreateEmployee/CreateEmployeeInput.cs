using MediatR;

namespace Odin.Baseline.Application.Employees.CreateEmployee
{
    public class CreateEmployeeInput : IRequest<EmployeeOutput>
    {        
        public Guid? DepartmentId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Document { get; private set; }
        public string Email { get; private set; }

        public CreateEmployeeInput(string firstName, string lastName, string document, string email, Guid? departmentId = null)
        {
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
        }

        public void ChangeDocument(string document)
        {
            Document = document;
        }
    }
}
