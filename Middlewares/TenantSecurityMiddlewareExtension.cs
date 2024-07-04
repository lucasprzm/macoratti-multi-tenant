namespace Macoratti_MultiTenant.Middlewares
{
    public static class TenantSecurityMiddlewareExtension
    {
        public static IApplicationBuilder UseTenant(this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantSecurityMiddleware>();
            return app;
        }
    }
}
