using Macoratti_MultiTenant.Repositories;

namespace Macoratti_MultiTenant.Middlewares
{
    public class TenantSecurityMiddleware
    {
        private readonly RequestDelegate next;

        public TenantSecurityMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration,
                   IHttpContextAccessor httpContextAccessor)
        {
            string tenantIdentifier = context.Session.GetString("TenantId");
            if (string.IsNullOrEmpty(tenantIdentifier))
            {
                var apiKey = context.Request.Headers["x-api-Key"].FirstOrDefault();
                if (string.IsNullOrEmpty(apiKey))
                {
                    return;
                }

                Guid apiKeyGuid;
                if (!Guid.TryParse(apiKey, out apiKeyGuid))
                {
                    return;
                }

                TenantRepository _tenentRepository = new TenantRepository(configuration,
                 httpContextAccessor);
                string tenantId = await _tenentRepository.GetTenantId(apiKeyGuid);
                context.Session.SetString("TenantId", tenantId);
            }
            await next.Invoke(context);
        }
    }
}
