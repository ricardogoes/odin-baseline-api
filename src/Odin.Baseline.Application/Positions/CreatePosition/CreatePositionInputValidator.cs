using FluentValidation;

namespace Odin.Baseline.Application.Positions.CreatePosition
{
    public class CreatePositionInputValidator
        : AbstractValidator<CreatePositionInput>
    {
        public CreatePositionInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
