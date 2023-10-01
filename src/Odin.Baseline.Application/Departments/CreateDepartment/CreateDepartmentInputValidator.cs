using FluentValidation;

namespace Odin.Baseline.Application.Departments.CreateDepartment
{
    public class CreateDepartmentInputValidator : AbstractValidator<CreateDepartmentInput>
    {
        public CreateDepartmentInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty();          
        }
    }
}
