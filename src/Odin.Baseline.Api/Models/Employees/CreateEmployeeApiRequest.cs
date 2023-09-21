namespace Odin.Baseline.Api.Models.Employees
{
    public class CreateEmployeeApiRequest
    {        
        public Guid CustomerId { get; private set; }
        public Guid? DepartmentId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Document { get; private set; }
        public string Email { get; private set; }

        public CreateEmployeeApiRequest(Guid customerId, string firstName, string lastName, string document, string email, Guid? departmentId = null)
        {
            CustomerId = customerId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
        }
    }
}
