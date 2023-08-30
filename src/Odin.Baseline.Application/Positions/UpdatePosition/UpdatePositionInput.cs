using MediatR;
using Odin.Baseline.Application.Positions.Common;

namespace Odin.Baseline.Application.Positions.UpdatePosition
{
    public class UpdatePositionInput : IRequest<PositionOutput>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public decimal? BaseSalary { get; set; }
        public string LoggedUsername { get; set; }
    }
}
