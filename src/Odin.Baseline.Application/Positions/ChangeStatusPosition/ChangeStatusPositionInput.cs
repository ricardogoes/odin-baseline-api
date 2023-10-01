using MediatR;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Positions.ChangeStatusPosition
{
    public class ChangeStatusPositionInput : IRequest<PositionOutput>
    {        
        public Guid Id { get; private set; }
        public ChangeStatusAction? Action { get; private set; }

        public ChangeStatusPositionInput(Guid id, ChangeStatusAction? action)
        {
            Id = id;
            Action = action;
        }
    }
}
