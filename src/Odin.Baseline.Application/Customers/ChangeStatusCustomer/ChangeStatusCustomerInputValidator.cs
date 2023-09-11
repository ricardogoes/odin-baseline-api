﻿using FluentValidation;

namespace Odin.Baseline.Application.Customers.ChangeStatusCustomer
{
    public class ChangeStatusCustomerInputValidator
        : AbstractValidator<ChangeStatusCustomerInput>
    {
        public ChangeStatusCustomerInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Action).NotEmpty();
            RuleFor(x => x.LoggedUsername).NotEmpty();
        }
    }
}