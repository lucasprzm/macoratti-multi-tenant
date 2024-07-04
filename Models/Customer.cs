namespace Macoratti_MultiTenant.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string? CustomerName { get; set; }
        public bool IsActive { get; set; }
    }
}
