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
        public string CreatedBy { get; set; }
        public DateTime? CreatedAtStart { get; set; }
        public DateTime? CreatedAtEnd { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAtStart { get; set; }
        public DateTime? LastUpdatedAtEnd { get; set; }

        public GetPositionsInput()
            : base()
        { }

        public GetPositionsInput(int page, int pageSize, string sort, Guid customerId, string name, bool? isActive,
            string createdBy, DateTime? createdAtStart, DateTime? createdAtEnd,
            string lastUpdatedBy, DateTime? lastUpdatedAtStart, DateTime? lastUpdatedAtEnd)
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
