using FluentValidation;

namespace Odin.Baseline.Application.Departments.ChangeStatusDepartment
{
    public class ChangeStatusDepartmentInputValidator : AbstractValidator<ChangeStatusDepartmentInput>
    {
        public ChangeStatusDepartmentInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Action).NotEmpty();
        }
    }
}
