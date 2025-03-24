using api.Data;
using api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ApplicationDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("register")]
        [Authorize]
        public ActionResult Register()
        {
            // Get user information from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID claim is missing");
            }

            // Check if user already exists
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (existingUser != null)
            {
                return Ok("User already registered");
            }

            // Create new user
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? $"{userId}@unknown.com";
            
            var newUser = new ApplicationUser
            {
                Id = userId,
                Name = userName ?? "Unknown User",
                Email = email
            };
            
            _context.Users.Add(newUser);
            _context.SaveChanges();
            
            _logger.LogInformation($"New user registered: {userId}");
            
            return Ok("User registered successfully");
        }
    }
} 