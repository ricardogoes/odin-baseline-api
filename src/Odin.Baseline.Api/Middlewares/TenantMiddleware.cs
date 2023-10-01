using Odin.Baseline.Domain.CustomExceptions;

namespace Odin.Baseline.Api.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;        
        
        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.HasValue 
                && !context.Request.Path.Value!.Contains("customer")
                && !context.Request.Path.Value!.Contains("sign-in")
                && !context.Request.Path.Value!.Contains("sign-out"))
            {
                context.Request.Headers.TryGetValue("X-TENANT-ID", out var tenantId);

                if (string.IsNullOrWhiteSpace(tenantId))
                    throw new BadRequestException("Invalid tenant");
            }

            await _next(context);
        }
    }
}
