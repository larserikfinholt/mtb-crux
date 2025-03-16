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
                },

                  new Crux { Lat = "59.2096", Lng = "9.6080", Level = 3, Name = "Skien Crux 1", Description = "Description 1" },
                new Crux { Lat = "59.2100", Lng = "9.6085", Level = 4, Name = "Skien Crux 2", Description = "Description 2" },
                new Crux { Lat = "59.2105", Lng = "9.6090", Level = 2, Name = "Skien Crux 3", Description = "Description 3" },
                new Crux { Lat = "59.2110", Lng = "9.6095", Level = 5, Name = "Skien Crux 4", Description = "Description 4" },
                new Crux { Lat = "59.2115", Lng = "9.6100", Level = 1, Name = "Skien Crux 5", Description = "Description 5" },
                new Crux { Lat = "59.2120", Lng = "9.6105", Level = 3, Name = "Skien Crux 6", Description = "Description 6" },
                new Crux { Lat = "59.2125", Lng = "9.6110", Level = 4, Name = "Skien Crux 7", Description = "Description 7" },
                new Crux { Lat = "59.2130", Lng = "9.6115", Level = 2, Name = "Skien Crux 8", Description = "Description 8" },
                new Crux { Lat = "59.2135", Lng = "9.6120", Level = 5, Name = "Skien Crux 9", Description = "Description 9" },
                new Crux { Lat = "59.2140", Lng = "9.6125", Level = 1, Name = "Skien Crux 10", Description = "Description 10" },
                new Crux { Lat = "59.2145", Lng = "9.6130", Level = 3, Name = "Skien Crux 11", Description = "Description 11" },
                new Crux { Lat = "59.2150", Lng = "9.6135", Level = 4, Name = "Skien Crux 12", Description = "Description 12" },
                new Crux { Lat = "59.2155", Lng = "9.6140", Level = 2, Name = "Skien Crux 13", Description = "Description 13" },
                new Crux { Lat = "59.2160", Lng = "9.6145", Level = 5, Name = "Skien Crux 14", Description = "Description 14" },
                new Crux { Lat = "59.2165", Lng = "9.6150", Level = 1, Name = "Skien Crux 15", Description = "Description 15" },
                new Crux { Lat = "59.2170", Lng = "9.6155", Level = 3, Name = "Skien Crux 16", Description = "Description 16" },
                new Crux { Lat = "59.2175", Lng = "9.6160", Level = 4, Name = "Skien Crux 17", Description = "Description 17" },
                new Crux { Lat = "59.2180", Lng = "9.6165", Level = 2, Name = "Skien Crux 18", Description = "Description 18" },
                new Crux { Lat = "59.2185", Lng = "9.6170", Level = 5, Name = "Skien Crux 19", Description = "Description 19" },
                new Crux { Lat = "59.2190", Lng = "9.6175", Level = 1, Name = "Skien Crux 20", Description = "Description 20" }


            };
        }
    }
}
