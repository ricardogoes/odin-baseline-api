using MediatR;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Application.Positions.ChangeStatusPosition
{
    public class ChangeStatusPositionInput : IRequest<PositionOutput>
    {        
        public Guid Id { get; private set; }
        public ChangeStatusAction? Action { get; private set; }
        public string LoggedUsername { get; private set; }

        public ChangeStatusPositionInput(Guid id, ChangeStatusAction? action, string loggedUsername)
        {
            Id = id;
            Action = action;
            LoggedUsername = loggedUsername;
        }
    }
}
