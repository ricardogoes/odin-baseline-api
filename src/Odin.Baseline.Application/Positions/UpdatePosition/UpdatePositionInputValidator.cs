﻿using FluentValidation;
using Odin.Baseline.Domain.Validations;

namespace Odin.Baseline.Application.Positions.UpdatePosition
{
    public class UpdatePositionInputValidator
        : AbstractValidator<UpdatePositionInput>
    {
        public UpdatePositionInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();            
            RuleFor(x => x.LoggedUsername).NotEmpty();
        }
    }
}
