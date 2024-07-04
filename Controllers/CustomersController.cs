using Macoratti_MultiTenant.Models;
using Macoratti_MultiTenant.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Macoratti_MultiTenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;
        private readonly TenantRepository _tenantRepository;
        public CustomersController(CustomerRepository customerRepository, TenantRepository tenantRepository)
        {
            _customerRepository = customerRepository;
            _tenantRepository = tenantRepository;

        }

        [HttpGet]
        public async Task<List<Customer>> GetAll()
        {
            string tenantId = await _tenantRepository.GetTenantId();
            return await _customerRepository.GetAllCustomers(tenantId);
        }
    }
}
