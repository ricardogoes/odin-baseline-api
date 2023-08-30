using FluentValidation;

namespace Odin.Baseline.Application.Employees.AddPosition
{
    public class AddPositionInputValidator : AbstractValidator<AddPositionInput>
    {
        public AddPositionInputValidator()
        {
            RuleFor(x => x.EmployeeId).NotNull().NotEmpty();
            RuleFor(x => x.PositionId).NotNull().NotEmpty();
            RuleFor(x => x.Salary).NotNull().NotEmpty();
            RuleFor(x => x.LoggedUsername).NotNull().NotEmpty();
        }
    }
}
