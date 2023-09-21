using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Common;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Positions.GetPositions
{
    public class GetPositions : IRequestHandler<GetPositionsInput, PaginatedListOutput<PositionOutput>>
    {
        private readonly IRepository<Position> _repository;
        private readonly IValidator<GetPositionsInput> _validator;

        public GetPositions(IRepository<Position> repository, IValidator<GetPositionsInput> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<PaginatedListOutput<PositionOutput>> Handle(GetPositionsInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
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
