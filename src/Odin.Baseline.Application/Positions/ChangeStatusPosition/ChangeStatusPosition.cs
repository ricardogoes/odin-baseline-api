using MediatR;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Positions.ChangeStatusPosition
{
    public class ChangeStatusPosition : IRequestHandler<ChangeStatusPositionInput, PositionOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Position> _repository;

        public ChangeStatusPosition(IUnitOfWork unitOfWork, IRepository<Position> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<PositionOutput> Handle(ChangeStatusPositionInput input, CancellationToken cancellationToken)
        {
            var position = await _repository.FindByIdAsync(input.Id, cancellationToken);

            switch (input.Action)
            {
                case ChangeStatusAction.ACTIVATE:
                    position.Activate(input.LoggedUsername);
                    break;
                case ChangeStatusAction.DEACTIVATE:
                    position.Deactivate(input.LoggedUsername);
                    break;
            }

            var positionUpdated = await _repository.UpdateAsync(position, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return PositionOutput.FromPosition(positionUpdated);
        }
    }
}
