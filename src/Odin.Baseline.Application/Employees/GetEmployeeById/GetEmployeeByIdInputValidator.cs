using FluentValidation;

namespace Odin.Baseline.Application.Employees.GetEmployeeById
{
    public class GetEmployeeByIdInputValidator
        : AbstractValidator<GetEmployeeByIdInput>
    {
        public GetEmployeeByIdInputValidator()
            => RuleFor(x => x.Id).NotEmpty();
    }
}
