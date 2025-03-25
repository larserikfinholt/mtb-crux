using api.Data;
using api.Model;
using api.Services;
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
        private readonly AuthService _authService;

        public AuthController(
            ApplicationDbContext context, 
            ILogger<AuthController> logger,
            AuthService authService)
        {
            _context = context;
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("register")]
        [Authorize]
        public async Task<ActionResult> Register()
        {
            try
            {
                // Log all available claims for debugging
                _logger.LogInformation("Available claims:");
                foreach (var claim in User.Claims)
                {
                    _logger.LogInformation($"Claim type: {claim.Type}, value: {claim.Value}");
                }

                // Get user ID from the token
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

                // Get the access token from the request
                var accessToken = HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(accessToken))
                {
                    return BadRequest("Access token is missing");
                }

                // Get complete user info from Auth0
                var userInfo = await _authService.GetUserInfoAsync(accessToken);
                _logger.LogInformation($"User info from Auth0: {userInfo.Email}, {userInfo.Name}");

                // Create new user with info from Auth0
                var newUser = new ApplicationUser
                {
                    Id = userId,
                    Name = userInfo.Name ?? "Unknown User",
                    Email = userInfo.Email ?? $"{userId}@unknown.com"
                };
                
                _context.Users.Add(newUser);
                _context.SaveChanges();
                
                _logger.LogInformation($"New user registered: {userId}");
                
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering user: {ex.Message}");
                return StatusCode(500, "An error occurred while registering the user");
            }
        }

        [HttpGet("user-info")]
        [Authorize]
        public async Task<ActionResult> GetUserInfo()
        {
            try
            {
                // Get the access token from the request
                var accessToken = HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(accessToken))
                {
                    return BadRequest("Access token is missing");
                }

                // Get complete user info from Auth0
                var userInfo = await _authService.GetUserInfoAsync(accessToken);
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user info: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving user info");
            }
        }
    }
} 