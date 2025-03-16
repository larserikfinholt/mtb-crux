using api.Model;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CruxController : ControllerBase
    {

        private readonly ILogger<CruxController> _logger;

        public CruxController(ILogger<CruxController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetCrux")]
        public IEnumerable<Crux> Get()
        {
            return new List<Crux>
            {
                new Crux
                {
                    Lat = "40.0150",
                    Lng = "-105.2705",
                    Level = 5,
                    Name = "The Bastille Crack",
                    Description = "The Bast"
                }
            };
        }
    }
}
