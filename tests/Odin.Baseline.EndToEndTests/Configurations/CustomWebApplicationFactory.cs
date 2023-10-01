using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Odin.Baseline.Infra.Data.EF;

namespace Odin.Baseline.EndToEndTests.Configurations
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var environment = "EndToEndTest";
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
            builder.UseEnvironment(environment);

            builder.ConfigureServices(services =>
            {
                var dbOptions = services.FirstOrDefault(x => x.ServiceType == typeof(DbContextOptions<OdinBaselineDbContext>));
                if (dbOptions != null)
                    services.Remove(dbOptions);

                services.AddDbContext<OdinBaselineDbContext>(options =>
                {
                    options.UseInMemoryDatabase("e2e-tests-db");
                });
            });

            base.ConfigureWebHost(builder);
        }
    }
}
