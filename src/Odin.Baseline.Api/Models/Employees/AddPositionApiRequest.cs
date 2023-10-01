namespace Odin.Baseline.Api.Models.Employees
{
    public class AddPositionApiRequest
    {

        public Guid PositionId { get; private set; }
        public decimal Salary { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? FinishDate { get; private set; }

        public AddPositionApiRequest(Guid positionId, decimal salary, DateTime startDate, DateTime? finishDate)
        {
            PositionId = positionId;
            Salary = salary;
            StartDate = startDate;
            FinishDate = finishDate;
        }
    }
}
