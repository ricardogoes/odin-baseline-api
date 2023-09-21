using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.SeedWork;
using Odin.Baseline.Domain.Validations;

namespace Odin.Baseline.Domain.Entities
{
    public class Department : Entity
    {
        public Guid CustomerId { get; private set; }
        public CustomerData? CustomerData { get; private set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }

        public Department(Guid id, Guid customerId, string name, bool isActive = true)
            : base(id)
        {
            CustomerId = customerId;
            Name = name;
            IsActive = isActive;

            Validate();
        }

        public Department(Guid customerId, string name, bool isActive = true)
        {
            CustomerId = customerId;
            Name = name;
            IsActive = isActive;

            Validate();
        }

        public void Create(string loggedUsername)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = loggedUsername;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

            Validate();
        }

        public void Update(string newName, Guid? newCustomerId, string loggedUsername)
        {
            CustomerId = newCustomerId ?? CustomerId;
            Name = newName;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

            Validate();
        }

        public void SetAuditLog(DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;

            Validate();
        }

        public void Activate(string loggedUsername)
        {
            IsActive = true;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

            Validate();
        }

        public void Deactivate(string loggedUsername)
        {
            IsActive = false;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

            Validate();
        }

        public void LoadCustomerData(CustomerData customerData)
        {
            CustomerData = customerData;
            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(CustomerId, nameof(CustomerId));
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        }
    }
}
