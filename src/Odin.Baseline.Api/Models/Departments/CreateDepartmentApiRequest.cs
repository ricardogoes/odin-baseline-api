namespace Odin.Baseline.Api.Models.Departments
{
    public class CreateDepartmentApiRequest
    {

        public Guid CustomerId { get; private set; }
        public string Name { get; private set; }

        public CreateDepartmentApiRequest(Guid customerId, string name)
        {
            CustomerId = customerId;
            Name = name;
        }
    }
}
