﻿using Odin.Baseline.Domain.SeedWork;
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

        public EmployeePositionHistory(Guid positionId, decimal salary, DateTime startDate, DateTime? finishDate = null, bool isActual = true)
        {
            PositionId = positionId;
            Salary = salary;
            StartDate = startDate;
            FinishDate = finishDate;
            IsActual = isActual;

            Validate();
        }

        public void UpdateFinishDate(DateTime finishDate)
        {
            FinishDate = finishDate;
            IsActual = false;
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
