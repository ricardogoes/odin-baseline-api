using Odin.Baseline.Domain.SeedWork;
using Odin.Baseline.Domain.Validations;

namespace Odin.Baseline.Domain.Entities
{
    public class Position : Entity
    {
        public string Name { get; private set; }
        public decimal? BaseSalary { get; private set; }
        public bool IsActive { get; private set; }

        public Position(Guid id, string name, decimal? baseSalary, bool isActive = true)
           : base(id)
        {
            Name = name;
            BaseSalary = baseSalary;
            IsActive = isActive;

            Validate();
        }

        public Position(string name, decimal? baseSalary, bool isActive = true)
        {
            Name = name;
            BaseSalary = baseSalary;
            IsActive = isActive;

            Validate();
        }

        public void Update(string newName, decimal? newBaseSalary)
        {
            Name = newName;
            BaseSalary = newBaseSalary ?? BaseSalary;

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
