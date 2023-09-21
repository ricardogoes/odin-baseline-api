using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;

namespace Odin.Baseline.Api.Configurations
{
    public static class SecurityConfiguration
    {

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKeycloakAuthentication(configuration);

            services
                .AddAuthorization()
                .AddKeycloakAuthorization(configuration);

            return services;
        }
    }
}
