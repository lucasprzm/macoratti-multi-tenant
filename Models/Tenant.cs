namespace Macoratti_MultiTenant.Models
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public Guid ApiKey { get; set; }
        public string? TenantName { get; set; }
        public bool IsActive { get; set; }
    }
}
