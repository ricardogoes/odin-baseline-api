using FluentValidation;

namespace Odin.Baseline.Application.Departments.GetDepartmentById
{
    public class GetDepartmentByIdInputValidator
        : AbstractValidator<GetDepartmentByIdInput>
    {
        public GetDepartmentByIdInputValidator()
            => RuleFor(x => x.Id).NotEmpty();
    }
}
