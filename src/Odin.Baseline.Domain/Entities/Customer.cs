using Odin.Baseline.Domain.SeedWork;
using Odin.Baseline.Domain.Validations;
using Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.Domain.Entities
{
    public class Customer : EntityWithDocument
    {
        public string Name { get; private set; }
        public Address? Address { get; private set; }
        public bool IsActive { get; private set; }

        //TODO: Implementar loggedUsername
        private const string LOGGED_USERNAME = "ricardo.goes";

        public Customer(Guid id, string name, string document, bool isActive = true)
            : base(document, id)
        {
            Name = name;
            IsActive = isActive;

            Validate();
        }

        public Customer(string name, string document, bool isActive = true)    
            : base(document)
        {
            Name = name;
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

        public void Update(string newName, string? newDocument = null, string loggedUsername = LOGGED_USERNAME)
        {
            Name = newName;
            Document = newDocument ?? Document;
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

        public void ChangeAddress(Address newAddress)
        {
            Address = newAddress;
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

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
            CpfCnpjValidation.CpfCnpj(Document, nameof(Document));
        }
    }
}
