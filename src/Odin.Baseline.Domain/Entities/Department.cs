using Odin.Baseline.Domain.SeedWork;
using Odin.Baseline.Domain.Validations;

namespace Odin.Baseline.Domain.Entities
{
    public class Department : Entity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public Department(Guid id, string name, bool isActive = true)
            : base(id)
        {
            Name = name;
            IsActive = isActive;

            Validate();
        }

        public Department(string name, bool isActive = true)
        {
            Name = name;
            IsActive = isActive;

            Validate();
        }

        public void Update(string newName)
        {
            Name = newName;
            Validate();
        }

        public void Activate()
        {
            IsActive = true;
            Validate();
        }

        public void Deactivate()
        {
            IsActive = false;
            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        }
    }
}
