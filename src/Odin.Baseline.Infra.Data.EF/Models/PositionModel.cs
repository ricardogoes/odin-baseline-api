namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class PositionModel
    {       
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal? BaseSalary { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string LastUpdatedBy { get; private set; }

        public Guid CustomerId { get; private set; }
        public CustomerModel? Customer { get; private set; }

        public PositionModel(Guid id, string name, decimal? baseSalary, bool isActive, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, Guid customerId)
        {
            Id = id;
            Name = name;
            BaseSalary = baseSalary;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            CustomerId = customerId;
        }

        public PositionModel(Guid id, string name, decimal? baseSalary, bool isActive, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, Guid customerId, CustomerModel customerModel)
        {
            Id = id;
            Name = name;
            BaseSalary = baseSalary;
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
