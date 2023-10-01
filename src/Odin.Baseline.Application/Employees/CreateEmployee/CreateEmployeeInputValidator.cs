using FluentValidation;
using Odin.Baseline.Domain.Validations;

namespace Odin.Baseline.Application.Employees.CreateEmployee
{
    public class CreateEmployeeInputValidator : AbstractValidator<CreateEmployeeInput>
    {
        public CreateEmployeeInputValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();

            RuleFor(x => x.Document)
                .Must(CpfCnpjValidation.IsCpfCnpj)
                .WithMessage("'Document' must be a valid CPF or CNPJ");

            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
