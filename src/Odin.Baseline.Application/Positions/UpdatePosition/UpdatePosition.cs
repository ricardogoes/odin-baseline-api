using MediatR;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Positions.UpdatePosition
{
    public class UpdatePosition : IRequestHandler<UpdatePositionInput, PositionOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Position> _repository;

        public UpdatePosition(IUnitOfWork unitOfWork, IRepository<Position> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<PositionOutput> Handle(UpdatePositionInput input, CancellationToken cancellationToken)
        {
            var position = await _repository.FindByIdAsync(input.Id, cancellationToken);
            position.Update(input.Name, input.CustomerId, input.BaseSalary, input.LoggedUsername);

            var positionUpdated = await _repository.UpdateAsync(position, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return PositionOutput.FromPosition(positionUpdated);
        }
    }
}
