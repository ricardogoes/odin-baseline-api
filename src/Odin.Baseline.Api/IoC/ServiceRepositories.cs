using Odin.Baseline.Data.Repositories;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Api.IoC
{
    public class ServiceRepositories : ServiceBase
    {
        protected override void HttpClient(IServiceCollection services)
        {
            base.HttpClient(services);
        }

        protected override void Scoped(IServiceCollection services)
        {
            services.AddScoped<ICustomersRepository, CustomersRepository>();
            base.Scoped(services);
        }
    }
}
