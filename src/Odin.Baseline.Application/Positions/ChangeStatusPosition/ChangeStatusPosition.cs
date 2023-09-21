using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Positions.ChangeStatusPosition
{
    public class ChangeStatusPosition : IRequestHandler<ChangeStatusPositionInput, PositionOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Position> _repository;
        private readonly IValidator<ChangeStatusPositionInput> _validator;

        public ChangeStatusPosition(IUnitOfWork unitOfWork, IRepository<Position> repository, IValidator<ChangeStatusPositionInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<PositionOutput> Handle(ChangeStatusPositionInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
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
