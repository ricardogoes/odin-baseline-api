namespace Odin.Baseline.Api.Models.Employees
{
    public class UpdateEmployeeApiRequest
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid? DepartmentId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Document { get; private set; }
        public string Email { get; private set; }

        public UpdateEmployeeApiRequest(Guid id, Guid customerId, string firstName, string lastName, string document, string email, Guid? departmentId = null)
        {
            Id = id;
            CustomerId = customerId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
        }
    }
}
