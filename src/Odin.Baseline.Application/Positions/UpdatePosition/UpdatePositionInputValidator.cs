using FluentValidation;

namespace Odin.Baseline.Application.Positions.UpdatePosition
{
    public class UpdatePositionInputValidator : AbstractValidator<UpdatePositionInput>
    {
        public UpdatePositionInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
