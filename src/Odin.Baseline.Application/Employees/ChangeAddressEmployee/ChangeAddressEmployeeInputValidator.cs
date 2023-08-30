using FluentValidation;

namespace Odin.Baseline.Application.Employees.ChangeAddressEmployee
{
    public class ChangeAddressEmployeeInputValidator : AbstractValidator<ChangeAddressEmployeeInput>
    {
        public ChangeAddressEmployeeInputValidator()
        {
            RuleFor(x => x.EmployeeId).NotNull().NotEmpty();
            RuleFor(x => x.StreetName).NotNull().NotEmpty();
            RuleFor(x => x.StreetNumber).NotNull().NotEmpty();
            RuleFor(x => x.Neighborhood).NotNull().NotEmpty();
            RuleFor(x => x.ZipCode).NotNull().NotEmpty();
            RuleFor(x => x.City).NotNull().NotEmpty();
            RuleFor(x => x.State).NotNull().NotEmpty();
        }
    }
}
