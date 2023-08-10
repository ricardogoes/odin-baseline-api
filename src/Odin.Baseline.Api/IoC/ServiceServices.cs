using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Service;

namespace Odin.Baseline.Api.IoC
{
    public class ServiceServices : ServiceBase
    {
        protected override void HttpClient(IServiceCollection services)
        {
            base.HttpClient(services);
        }

        protected override void Scoped(IServiceCollection services)
        {
            services.AddScoped<ICustomersService, CustomersService>();
            base.Scoped(services);
        }
    }
}
