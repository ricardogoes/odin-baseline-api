using MediatR;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Positions.CreatePosition
{
    public class CreatePosition : IRequestHandler<CreatePositionInput, PositionOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Position> _repository;

        public CreatePosition(IUnitOfWork unitOfWork, IRepository<Position> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<PositionOutput> Handle(CreatePositionInput input, CancellationToken cancellationToken)
        {
            var position = new Position(input.CustomerId, input.Name, input.BaseSalary);
            position.Create(input.LoggedUsername);

            await _repository.InsertAsync(position, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return PositionOutput.FromPosition(position);
        }
    }
}
