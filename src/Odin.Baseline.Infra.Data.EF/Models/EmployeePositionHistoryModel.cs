namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class EmployeePositionHistoryModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        
        public Guid PositionId { get; set; }
        public PositionModel Position { get; set; }

        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public bool IsActual { get; set; }
    }
}
