using MediatR;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Positions.ChangeStatusPosition
{
    public class ChangeStatusPositionInput : IRequest<PositionOutput>
    {
        public Guid Id { get; set; }
        public ChangeStatusAction? Action { get; set; }
        public string LoggedUsername { get; set; }
    }
}
