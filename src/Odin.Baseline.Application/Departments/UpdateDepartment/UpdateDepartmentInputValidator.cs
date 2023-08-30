﻿using FluentValidation;

namespace Odin.Baseline.Application.Departments.UpdateDepartment
{
    public class UpdateDepartmentInputValidator
        : AbstractValidator<UpdateDepartmentInput>
    {
        public UpdateDepartmentInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();            
            RuleFor(x => x.LoggedUsername).NotEmpty();
        }
    }
}
