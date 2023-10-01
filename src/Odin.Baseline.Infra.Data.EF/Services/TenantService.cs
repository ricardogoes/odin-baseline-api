using Microsoft.AspNetCore.Http;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Infra.Data.EF.Services
{
    public class TenantService : ITenantService
    {
        private readonly Guid TenantId;
        
        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            if(!string.IsNullOrWhiteSpace(httpContextAccessor?.HttpContext?.Request.Headers["X-TENANT-ID"]))
                TenantId = Guid.Parse(httpContextAccessor.HttpContext.Request.Headers["X-TENANT-ID"]!);
            else
                // Used for tests only
                TenantId = Guid.Parse("5F9B7808-803F-4985-9996-6EBA9003F9CD");
        }

        public Guid GetTenant()
            => TenantId;
    }
}
