using FluentValidation;

namespace Odin.Baseline.Application.Positions.GetPositionById
{
    public class GetPositionByIdInputValidator
        : AbstractValidator<GetPositionByIdInput>
    {
        public GetPositionByIdInputValidator()
            => RuleFor(x => x.Id).NotEmpty();
    }
}
