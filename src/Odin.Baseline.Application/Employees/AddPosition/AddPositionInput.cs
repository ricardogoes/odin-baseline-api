using MediatR;
using Odin.Baseline.Application.Employees.Common;

namespace Odin.Baseline.Application.Employees.AddPosition
{
    public class AddPositionInput : IRequest<EmployeeOutput>
    {       

        public Guid EmployeeId { get; private set; }
        public Guid PositionId { get; private set; }
        public decimal Salary { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? FinishDate { get; private set; }
        public string LoggedUsername { get; private set; }

        public AddPositionInput(Guid employeeId, Guid positionId, decimal salary, DateTime startDate, DateTime? finishDate, string loggedUsername)
        {
            EmployeeId = employeeId;
            PositionId = positionId;
            Salary = salary;
            StartDate = startDate;
            FinishDate = finishDate;
            LoggedUsername = loggedUsername;
        }

        public void ChangeEmployeeId(Guid employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
