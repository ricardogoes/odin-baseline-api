using FluentValidation;
using MediatR;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Positions.UpdatePosition
{
    public class UpdatePosition : IRequestHandler<UpdatePositionInput, PositionOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Position> _repository;
        private readonly IValidator<UpdatePositionInput> _validator;

        public UpdatePosition(IUnitOfWork unitOfWork, IRepository<Position> repository, IValidator<UpdatePositionInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<PositionOutput> Handle(UpdatePositionInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var position = await _repository.FindByIdAsync(input.Id, cancellationToken);
            position.Update(input.Name, input.BaseSalary);

            var positionUpdated = await _repository.UpdateAsync(position, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return PositionOutput.FromPosition(positionUpdated);
        }
    }
}
