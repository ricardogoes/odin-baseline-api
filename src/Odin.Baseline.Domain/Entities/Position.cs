using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.SeedWork;
using Odin.Baseline.Domain.Validations;

namespace Odin.Baseline.Domain.Entities
{
    public class Position : Entity
    {
        public Guid CustomerId { get; private set; }
        public CustomerData CustomerData { get; private set; }
        public string Name { get; private set; }
        public decimal? BaseSalary { get; private set; }
        public bool IsActive { get; private set; }


        //TODO: Implementar loggedUsername
        private const string LOGGED_USERNAME = "ricardo.goes";

        public Position(Guid id, Guid customerId, string name, decimal? baseSalary, bool isActive = true)
           : base(id)
        {
            CustomerId = customerId;
            Name = name;
            BaseSalary = baseSalary;
            IsActive = isActive;

            Validate();
        }

        public Position(Guid customerId, string name, decimal? baseSalary, bool isActive = true)
        {
            CustomerId = customerId;
            Name = name;
            BaseSalary = baseSalary;
            IsActive = isActive;

            Validate();
        }

        public void Create(string loggedUsername = LOGGED_USERNAME)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = loggedUsername; //TODO: Implementar loggedUser
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername; //TODO: Implementar loggedUser

            Validate();
        }

        public void Update(string newName, Guid? newCustomerId = null, decimal? newBaseSalary = null, string loggedUsername = LOGGED_USERNAME)
        {
            CustomerId = newCustomerId ?? CustomerId;
            Name = newName;
            BaseSalary = newBaseSalary ?? BaseSalary;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername; //TODO: Implementar loggedUser

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

        public void Activate(string loggedUsername = LOGGED_USERNAME)
        {
            IsActive = true;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername; //TODO: Implementar loggedUser

            Validate();
        }

        public void Deactivate(string loggedUsername = LOGGED_USERNAME)
        {
            IsActive = false;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername; //TODO: Implementar loggedUser

            Validate();
        }

        public void LoadCustomerData(CustomerData customerData)
        {
            CustomerData = customerData;
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(CustomerId, nameof(CustomerId));
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        }
    }
}
