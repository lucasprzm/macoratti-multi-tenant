﻿using System.Data.SqlClient;
using System.Data;
using Macoratti_MultiTenant.Models;
using Newtonsoft.Json;

namespace Macoratti_MultiTenant.Repositories
{
    public class TenantRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IConfiguration _configuration;

        public TenantRepository(IConfiguration configuration,
                     IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetTenantId(Guid apiKey)
        {
            string tenantId = null;
            try
            {
                using (var connection = new SqlConnection(_configuration
                                ["ConnectionStrings:TenantDbConnection"]))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(
                             "SELECT Id FROM Tenant WHERE ApiKey = @apiKey", connection))
                    {
                        command.Parameters.AddWithValue("@apiKey", apiKey);
                        var reader = await command.ExecuteReaderAsync();
                        if (reader.Read())
                        {
                            tenantId = reader["Id"].ToString();
                        }

                        if (!reader.IsClosed) await reader.CloseAsync();
                        if (connection.State != ConnectionState.Closed)
                            await connection.CloseAsync();

                        return tenantId;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> GetTenantId()
        {
            return await Task.FromResult(_session.GetString("TenantId"));
        }

        public async Task<string> GetAllTenantsAndCustomers()
        {
            string result = null;

            try
            {
                using (var connection = new SqlConnection(_configuration["ConnectionStrings:TenantDbConnection"]))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("SELECT Tenant.Id, Tenant.ApiKey, Tenant.TenantName, Customer.CustomerName FROM Tenant LEFT JOIN Customer ON Tenant.Id = Customer.TenantId FOR JSON AUTO, INCLUDE_NULL_VALUES",
                      connection))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            result = reader[0].ToString();
                        }

                        if (!reader.IsClosed) await reader.CloseAsync();
                    }
                    if (connection.State != ConnectionState.Closed) await connection.CloseAsync();

                    return FormatJsonData(result);
                }
            }
            catch
            {
                return null;
            }
        }
        private string FormatJsonData(string json)
        {
            dynamic data = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
