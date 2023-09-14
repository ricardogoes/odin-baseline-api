namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class DepartmentModel
    {
        public Guid Id { get; private set; }        
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string LastUpdatedBy { get; private set; }

        public Guid CustomerId { get; private set; }
        public CustomerModel? Customer { get; private set; }

        public ICollection<EmployeeModel> Employees { get; } = new List<EmployeeModel>();

        public DepartmentModel(Guid id, string name, bool isActive, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, Guid customerId)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            CustomerId = customerId;
        }

        public DepartmentModel(Guid id, string name, bool isActive, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, Guid customerId, CustomerModel customerModel)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            CustomerId = customerId;
            Customer = customerModel;
        }
    }
}
