using MediatR;
using Odin.Baseline.Application.Positions.Common;

namespace Odin.Baseline.Application.Positions.GetPositionById
{
    public class GetPositionByIdInput : IRequest<PositionOutput>
    {
        public Guid Id { get; set; }
    }
}
