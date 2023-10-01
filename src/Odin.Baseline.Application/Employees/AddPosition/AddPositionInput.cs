using MediatR;

namespace Odin.Baseline.Application.Employees.AddPosition
{
    public class AddPositionInput : IRequest<EmployeeOutput>
    {       

        public Guid EmployeeId { get; private set; }
        public Guid PositionId { get; private set; }
        public decimal Salary { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? FinishDate { get; private set; }

        public AddPositionInput(Guid employeeId, Guid positionId, decimal salary, DateTime startDate, DateTime? finishDate)
        {
            EmployeeId = employeeId;
            PositionId = positionId;
            Salary = salary;
            StartDate = startDate;
            FinishDate = finishDate;
        }

        public void ChangeEmployeeId(Guid employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
