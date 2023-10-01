using Microsoft.AspNetCore.Http;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Infra.Data.EF.Repositories
{
    public abstract class BaseRepository
    {        
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITenantService _tenantService;

        public BaseRepository(IHttpContextAccessor httpContextAccessor, ITenantService tenantService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tenantService = tenantService;
        }

        public Guid GetTenantId()
            => _tenantService.GetTenant();

        public string GetCurrentUsername()
        {
            if (!string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext.User?.Identity?.Name))
                return _httpContextAccessor.HttpContext.User.Identity.Name!;
            else
                return "Anonymous";
        }
    }
}
