using FluentValidation;

namespace Odin.Baseline.Application.Customers.ChangeAddressCustomer
{
    public class ChangeAddressCustomerInputValidator : AbstractValidator<ChangeAddressCustomerInput>
    {
        public ChangeAddressCustomerInputValidator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
            RuleFor(x => x.StreetName).NotNull().NotEmpty();
            RuleFor(x => x.StreetNumber).NotNull().NotEmpty();
            RuleFor(x => x.Neighborhood).NotNull().NotEmpty();
            RuleFor(x => x.ZipCode).NotNull().NotEmpty();
            RuleFor(x => x.City).NotNull().NotEmpty();
            RuleFor(x => x.State).NotNull().NotEmpty();
        }
    }
}
