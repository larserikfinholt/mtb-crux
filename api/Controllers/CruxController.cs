using api.Data;
using api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CruxController : ControllerBase
    {
        private readonly ILogger<CruxController> _logger;
        private readonly ApplicationDbContext _context;

        public CruxController(ILogger<CruxController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetCrux")]
        public IEnumerable<Crux> Get()
        {
            return _context.Cruxes.Include(x=>x.CreatedBy).ToList();
        }

        [HttpPost(Name = "CreateCrux")]
        [Authorize]
        public ActionResult<int> Create([FromBody] CruxDto cruxDto)
        {
            if (cruxDto == null)
            {
                return BadRequest();
            }

            // Get the user's ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // Create a new Crux entity from the DTO
            var newCrux = new Crux
            {
                Lat = cruxDto.Lat,
                Lng = cruxDto.Lng,
                Level = cruxDto.Level,
                Name = cruxDto.Name,
                Description = cruxDto.Description,
                CreatedById = userId
            };

            // Try to find existing user or create a new one if needed
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (existingUser == null && userId != null)
            {
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                var email = User.FindFirst(ClaimTypes.Email)?.Value ?? $"{userId}@unknown.com";
                
                var newUser = new ApplicationUser
                {
                    Id = userId,
                    Name = userName ?? "Unknown User",
                    Email = email
                };
                
                _context.Users.Add(newUser);
            }

            _context.Cruxes.Add(newCrux);
            _context.SaveChanges();

            return Ok(newCrux.Id);
        }
    }
}
