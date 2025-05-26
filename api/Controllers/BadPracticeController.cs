using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using api.Model;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadPracticeController : ControllerBase
    {
        // Hard-coded connection string with credentials
        private readonly string _connectionString = "Server=prod-sql.example.com;Database=Production;User Id=admin;Password=SuperSecretPassword123!";
        
        // No dependency injection, creating service directly
        private readonly HttpClient _httpClient = new HttpClient();
        
        // Hard-coded API key
        private readonly string _apiKey = "sk_live_abcdefg123456789APIKEY";
        
        // Missing authorization attribute for sensitive operation
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = new List<ApplicationUser>();
            
            // SQL injection vulnerability
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Users WHERE LastLoginDate > '" + Request.Query["date"] + "'";
                
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            users.Add(new ApplicationUser 
                            { 
                                Id = reader["Id"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                Email = reader["Email"].ToString()
                            });
                        }
                    }
                }
            }
            
            return Ok(users);
        }
        
        // Improper exception handling
        [HttpPost("process")]
        public async Task<IActionResult> ProcessData([FromBody] object data)
        {
            try
            {
                // Hardcoded URL
                var response = await _httpClient.PostAsync(
                    "https://api.example.com/v1/process",
                    new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
                );
                
                response.EnsureSuccessStatusCode();
                return Ok(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                // Exposing exception details to user
                return StatusCode(500, ex.ToString());
            }
        }
        
        // Bypass authentication with a backdoor
        [HttpGet("backdoor")]
        public IActionResult AdminBackdoor()
        {
            // Bypass normal authentication
            if (Request.Headers.TryGetValue("SuperSecretHeader", out var headerValue))
            {
                if (headerValue == "BypassAuth123!")
                {
                    return Ok(new { isAdmin = true, message = "You've accessed the admin backdoor!" });
                }
            }
            
            return Unauthorized();
        }
        
        // No authentication, accessing sensitive data
        [HttpGet("logs")]
        public IActionResult GetSystemLogs()
        {
            // No permissions check
            return Ok(new { message = "Here are all system logs including security events and PII data!" });
        }
        
        // Excessive data exposure
        [Authorize]
        [HttpGet("user/{id}")]
        public IActionResult GetUserDetails(string id)
        {
            // Returning everything including sensitive data
            return Ok(new { 
                userId = id,
                username = "user" + id,
                password = "hashedPassword123", // Not really hashed!
                ssn = "123-45-6789",
                creditCardNumber = "4111-1111-1111-1111",
                cvv = "123"
            });
        }
    }
}
