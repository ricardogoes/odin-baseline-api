using MediatR;

namespace Odin.Baseline.Application.Positions.GetPositionById
{
    public class GetPositionByIdInput : IRequest<PositionOutput>
    {
        public Guid Id { get; set; }
    }
}
