﻿using FluentValidation;
using Odin.Baseline.Api.Models.Customers;
using Odin.Baseline.Application.Customers.CreateCustomer;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Services;

namespace Odin.Baseline.Api.Configurations
{
    public static class ApplicationConfiguration
    {

        public static IServiceCollection AddApplications(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateCustomer).Assembly);
            });


            ValidatorOptions.Global.LanguageManager.Enabled = false;

            services.AddValidatorsFromAssemblyContaining<CreateCustomerInputValidator>();

            //Domain Services
            services.AddTransient<IDocumentService, DocumentService> ();

            return services;
        }
    }
}
