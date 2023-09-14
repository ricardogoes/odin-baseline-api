using MediatR;
using Odin.Baseline.Application.Common;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Positions.GetPositions
{
    public class GetPositions : IRequestHandler<GetPositionsInput, PaginatedListOutput<PositionOutput>>
    {
        private readonly IRepository<Position> _repository;

        public GetPositions(IRepository<Position> repository)
            => _repository = repository;

        public async Task<PaginatedListOutput<PositionOutput>> Handle(GetPositionsInput input, CancellationToken cancellationToken)
        {
            var filters = new Dictionary<string, object?>
            {
                { "CustomerId", input.CustomerId },
                { "Name", input.Name },
                { "IsActive", input.IsActive },
            };

            var positions = await _repository.FindPaginatedListAsync(
                filters, input.PageNumber, input.PageSize, input.Sort!,
                cancellationToken: cancellationToken);

            return new PaginatedListOutput<PositionOutput>
            (
                pageNumber: input.PageNumber,
                pageSize: input.PageSize,
                totalPages: PaginatedListHelper.GetTotalPages(positions.TotalItems, input.PageSize),
                totalItems: positions.TotalItems,
                items: PositionOutput.FromPosition(positions.Items)
            );
        }
    }
}
