using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Positions.CreatePosition
{
    public class CreatePosition : IRequestHandler<CreatePositionInput, PositionOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Position> _repository;
        private readonly IValidator<CreatePositionInput> _validator;

        public CreatePosition(IUnitOfWork unitOfWork, IRepository<Position> repository, IValidator<CreatePositionInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<PositionOutput> Handle(CreatePositionInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var position = new Position(input.CustomerId, input.Name, input.BaseSalary);
            position.Create(input.LoggedUsername);

            await _repository.InsertAsync(position, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return PositionOutput.FromPosition(position);
        }
    }
}
