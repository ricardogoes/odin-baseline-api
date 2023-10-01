using FluentValidation;
using Odin.Baseline.Application.Departments.CreateDepartment;
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
                cfg.RegisterServicesFromAssembly(typeof(CreateDepartment).Assembly);
            });


            ValidatorOptions.Global.LanguageManager.Enabled = false;

            services.AddValidatorsFromAssemblyContaining<CreateDepartmentInputValidator>();

            //Domain Services
            services.AddTransient<IDocumentService, DocumentService> ();

            return services;
        }
    }
}
