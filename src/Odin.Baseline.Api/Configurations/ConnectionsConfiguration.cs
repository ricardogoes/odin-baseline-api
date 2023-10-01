using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Domain.Models.AppSettings;
using Odin.Baseline.Infra.Data.EF;

namespace Odin.Baseline.Api.Configurations
{
    public static class ConnectionsConfiguration
    {
        public static IServiceCollection AddAppConnections(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddDbConnection(appSettings);
            return services;
        }

        private static IServiceCollection AddDbConnection(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddDbContext<OdinBaselineDbContext>(options =>
            {
                options.UseNpgsql(appSettings.ConnectionStringsSettings!.OdinBaselineDbConnection);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }
    }
}
