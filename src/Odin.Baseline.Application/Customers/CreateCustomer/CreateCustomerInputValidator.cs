using FluentValidation;
using Odin.Baseline.Domain.Validations;

namespace Odin.Baseline.Application.Customers.CreateCustomer
{
    public class CreateCustomerInputValidator
        : AbstractValidator<CreateCustomerInput>
    {
        public CreateCustomerInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Document)
                .Must(CpfCnpjValidation.IsCpfCnpj)
                .WithMessage("Document should be a valid CPF or CNPJ");
            
            RuleFor(x => x.LoggedUsername).NotEmpty();
        }
    }
}
