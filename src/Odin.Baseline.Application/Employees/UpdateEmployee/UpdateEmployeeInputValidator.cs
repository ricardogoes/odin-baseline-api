using FluentValidation;
using Odin.Baseline.Domain.Validations;

namespace Odin.Baseline.Application.Employees.UpdateEmployee
{
    public class UpdateEmployeeInputValidator
        : AbstractValidator<UpdateEmployeeInput>
    {
        public UpdateEmployeeInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty(); 
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Document).Must(CpfCnpjValidation.IsCpfCnpj).WithMessage("Document should be a valid CPF or CNPJ");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.LoggedUsername).NotEmpty();
        }
    }
}
