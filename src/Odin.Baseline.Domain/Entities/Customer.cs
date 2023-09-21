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

        public void Create(string loggedUsername)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = loggedUsername;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

            Validate();
        }

        public void Update(string newName, string? newDocument, string loggedUsername)
        {
            Name = newName;
            Document = newDocument ?? Document;
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

        public void ChangeAddress(Address newAddress, string loggedUsername)
        {
            Address = newAddress;

            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

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

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
            CpfCnpjValidation.CpfCnpj(Document, nameof(Document));
        }
    }
}
