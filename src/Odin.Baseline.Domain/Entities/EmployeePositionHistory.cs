using Odin.Baseline.Domain.SeedWork;
using Odin.Baseline.Domain.Validations;

namespace Odin.Baseline.Domain.Entities
{
    public class EmployeePositionHistory : Entity
    {
        public Guid PositionId { get; private set; }
        public decimal Salary { get; private set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public bool IsActual { get; private set; }

        //TODO: Implementar loggedUsername
        private const string LOGGED_USERNAME = "ricardo.goes";

        public EmployeePositionHistory(Guid positionId, decimal salary, DateTime startDate, DateTime? finishDate = null, bool isActual = true)
        {
            PositionId = positionId;
            Salary = salary;
            StartDate = startDate;
            FinishDate = finishDate;
            IsActual = isActual;

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

        public void UpdateFinishDate(DateTime finishDate, string loggedUsername = LOGGED_USERNAME)
        {
            FinishDate = finishDate;
            IsActual = false;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername; //TODO: Implementar loggedUser
            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(PositionId, nameof(PositionId));
            DomainValidation.NotNullOrEmpty(Salary, nameof(Salary));
            DomainValidation.NotNullOrEmpty(StartDate, nameof(StartDate));
        }
    }
}
