namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class EmployeePositionHistoryModel
    {
        public Guid EmployeeId { get; private set; }
        public EmployeeModel? Employee { get; private set; }
        
        public Guid PositionId { get; private set; }
        public PositionModel? Position { get; private set; }

        public decimal Salary { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? FinishDate { get; private set; }
        public bool IsActual { get; private set; }

        public EmployeePositionHistoryModel(Guid employeeId, Guid positionId, decimal salary, DateTime startDate, DateTime? finishDate, bool isActual)
        {
            EmployeeId = employeeId;
            PositionId = positionId;
            Salary = salary;
            StartDate = startDate;
            FinishDate = finishDate;
            IsActual = isActual;
        }
    }
}
