using FluentValidation;

namespace Odin.Baseline.Application.Employees.ChangeStatusEmployee
{
    public class ChangeStatusEmployeeInputValidator : AbstractValidator<ChangeStatusEmployeeInput>
    {
        public ChangeStatusEmployeeInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Action).NotEmpty();
        }
    }
}
