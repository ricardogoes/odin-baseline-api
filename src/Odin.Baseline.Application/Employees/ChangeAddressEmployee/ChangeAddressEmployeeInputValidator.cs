using FluentValidation;

namespace Odin.Baseline.Application.Employees.ChangeAddressEmployee
{
    public class ChangeAddressEmployeeInputValidator : AbstractValidator<ChangeAddressEmployeeInput>
    {
        public ChangeAddressEmployeeInputValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.StreetName).NotEmpty();
            RuleFor(x => x.StreetNumber).GreaterThan(0);
            RuleFor(x => x.Neighborhood).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
        }
    }
}
