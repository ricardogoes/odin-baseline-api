namespace Odin.Baseline.Api.Models.Departments
{
    public class UpdateDepartmentApiRequest
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public string Name { get; private set; }

        public UpdateDepartmentApiRequest(Guid id, Guid customerId, string name)
        {
            Id = id;
            CustomerId = customerId;
            Name = name;
        }
    }
}
