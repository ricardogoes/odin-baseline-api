using MediatR;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.DTO.Common;

namespace Odin.Baseline.Application.Positions.GetPositions
{
    public class GetPositionsInput : PaginatedListInput, IRequest<PaginatedListOutput<PositionOutput>>
    {
        public Guid CustomerId { get; private set; }
        public string? Name { get; private set; }
        public bool? IsActive { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime? CreatedAtStart { get; private set; }
        public DateTime? CreatedAtEnd { get; private set; }
        public string? LastUpdatedBy { get; private set; }
        public DateTime? LastUpdatedAtStart { get; private set; }
        public DateTime? LastUpdatedAtEnd { get; private set; }

        public GetPositionsInput()
            : base()
        { }

        public GetPositionsInput(int page, int pageSize, Guid customerId, string? sort = null, string? name = null, bool? isActive = null,
            string? createdBy = null, DateTime? createdAtStart = null, DateTime? createdAtEnd = null,
            string? lastUpdatedBy = null, DateTime? lastUpdatedAtStart = null, DateTime? lastUpdatedAtEnd = null)
            : base(page, pageSize, sort)
        {
            CustomerId = customerId;
            Name = name;
            IsActive = isActive;

            CreatedBy = createdBy;
            CreatedAtStart = createdAtStart;
            CreatedAtEnd = createdAtEnd;

            LastUpdatedBy = lastUpdatedBy;
            LastUpdatedAtStart = lastUpdatedAtStart;
            LastUpdatedAtEnd = lastUpdatedAtEnd;
        }
    }
}
