using Macoratti_MultiTenant.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Macoratti_MultiTenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly TenantRepository _tenantRepository;
        public TenantController(TenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return await _tenantRepository.GetAllTenantsAndCustomers();
        }
    }
}
