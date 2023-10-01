using FluentValidation;
using MediatR;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Positions.GetPositionById
{
    public class GetPositionById : IRequestHandler<GetPositionByIdInput, PositionOutput>
    {
        private readonly IRepository<Position> _repository;
        private readonly IValidator<GetPositionByIdInput> _validator;

        public GetPositionById(IRepository<Position> repository, IValidator<GetPositionByIdInput> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<PositionOutput> Handle(GetPositionByIdInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var position = await _repository.FindByIdAsync(input.Id, cancellationToken);
            return PositionOutput.FromPosition(position);
        }
    }
}
