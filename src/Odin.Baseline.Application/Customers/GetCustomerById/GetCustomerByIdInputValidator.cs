using FluentValidation;

namespace Odin.Baseline.Application.Customers.GetCustomerById
{
    public class GetCustomerByIdInputValidator
        : AbstractValidator<GetCustomerByIdInput>
    {
        public GetCustomerByIdInputValidator()
            => RuleFor(x => x.Id).NotEmpty();
    }
}
