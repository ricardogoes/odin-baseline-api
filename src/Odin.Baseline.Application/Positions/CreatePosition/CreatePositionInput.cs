using MediatR;
using Odin.Baseline.Application.Positions.Common;

namespace Odin.Baseline.Application.Positions.CreatePosition
{
    public class CreatePositionInput : IRequest<PositionOutput>
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public decimal? BaseSalary { get; set; }
        public string LoggedUsername { get; set; }
    }
}
