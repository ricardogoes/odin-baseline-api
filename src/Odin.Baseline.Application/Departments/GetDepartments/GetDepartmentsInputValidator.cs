using FluentValidation;
using Odin.Baseline.Application.Departments.GetDepartments;

namespace Odin.Baseline.Application.Departments.GetDepartments
{
    public class GetDepartmentsInputValidator : AbstractValidator<GetDepartmentsInput>
    {
        public GetDepartmentsInputValidator()
            => RuleFor(x => x.CustomerId).NotEmpty();
    }
}
