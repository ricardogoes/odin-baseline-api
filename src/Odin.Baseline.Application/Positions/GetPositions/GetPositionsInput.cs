using MediatR;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.DTO.Common;

namespace Odin.Baseline.Application.Positions.GetPositions
{
    public class GetPositionsInput : PaginatedListInput, IRequest<PaginatedListOutput<PositionOutput>>
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public GetPositionsInput()
        { }

        public GetPositionsInput(int page, int pageSize, string sort, Guid customerId, string name, bool? isActive)
            : base(page, pageSize, sort)
        {
            CustomerId = customerId;
            Name = name;
            IsActive = isActive;
        }
    }
}
