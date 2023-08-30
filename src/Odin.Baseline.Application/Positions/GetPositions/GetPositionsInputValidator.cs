using FluentValidation;

namespace Odin.Baseline.Application.Positions.GetPositions
{
    public class GetPositionsInputValidator : AbstractValidator<GetPositionsInput>
    {
        public GetPositionsInputValidator()
            => RuleFor(x => x.CustomerId).NotEmpty();
    }
}
