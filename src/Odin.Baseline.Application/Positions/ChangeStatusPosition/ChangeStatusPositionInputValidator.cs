using FluentValidation;

namespace Odin.Baseline.Application.Positions.ChangeStatusPosition
{
    public class ChangeStatusPositionInputValidator : AbstractValidator<ChangeStatusPositionInput>
    {
        public ChangeStatusPositionInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Action).NotEmpty();
        }
    }
}
