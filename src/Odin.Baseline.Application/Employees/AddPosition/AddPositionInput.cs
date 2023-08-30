using MediatR;
using Odin.Baseline.Application.Employees.Common;

namespace Odin.Baseline.Application.Employees.AddPosition
{
    public class AddPositionInput : IRequest<EmployeeOutput>
    {
        public Guid EmployeeId { get; set; }
        public Guid PositionId { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string LoggedUsername { get; set; }
    }
}
