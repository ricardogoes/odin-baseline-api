using MediatR;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Positions.GetPositionById
{
    public class GetPositionById : IRequestHandler<GetPositionByIdInput, PositionOutput>
    {
        private readonly IRepository<Position> _repository;

        public GetPositionById(IRepository<Position> repository)
        {
            _repository = repository;
        }

        public async Task<PositionOutput> Handle(GetPositionByIdInput input, CancellationToken cancellationToken)
        {
            var position = await _repository.FindByIdAsync(input.Id, cancellationToken);
            return PositionOutput.FromPosition(position);
        }
    }
}
