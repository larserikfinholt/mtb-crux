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
                    Id = 1,
                    Lat = "40.0150",
                    Lng = "-105.2705",
                    Level = 5,
                    Name = "The Bastille Crack",
                    Description = "The Bastille Crack is a Eldorado Canyon classic, featuring traditional climbing up a prominent dihedral."
                },

                new Crux { Id = 2, Lat = "59.2096", Lng = "9.6080", Level = 3, Name = "Vestveggen", Description = "Popular route with varied climbing on the west-facing wall" },
                new Crux { Id = 3, Lat = "59.2100", Lng = "9.6085", Level = 4, Name = "Fossekallen", Description = "Technical face climbing along the waterfall route" },
                new Crux { Id = 4, Lat = "59.2105", Lng = "9.6090", Level = 2, Name = "Granittblokka", Description = "Easy slab climbing on solid granite" },
                new Crux { Id = 5, Lat = "59.2110", Lng = "9.6095", Level = 5, Name = "Drømmefossen", Description = "Challenging overhang with exposed moves near the waterfall" },
                new Crux { Id = 6, Lat = "59.2115", Lng = "9.6100", Level = 1, Name = "Nybegynneren", Description = "Perfect beginner route with large holds and gentle incline" },
                new Crux { Id = 7, Lat = "59.2120", Lng = "9.6105", Level = 3, Name = "Sørveggen", Description = "South-facing wall with interesting crack systems" },
                new Crux { Id = 8, Lat = "59.2125", Lng = "9.6110", Level = 4, Name = "Ørneredet", Description = "High exposure climb with amazing views from the Eagle's Nest" },
                new Crux { Id = 9, Lat = "59.2130", Lng = "9.6115", Level = 2, Name = "Skogstien", Description = "Pleasant climb through pine forest terrain" },
                new Crux { Id = 10, Lat = "59.2135", Lng = "9.6120", Level = 5, Name = "Himmelstigen", Description = "Extremely steep route requiring strong finger strength" },
                new Crux { Id = 11, Lat = "59.2140", Lng = "9.6125", Level = 1, Name = "Fjellhylla", Description = "Easy traversing route along a natural mountain shelf" },
                new Crux { Id = 12, Lat = "59.2145", Lng = "9.6130", Level = 3, Name = "Regnbuen", Description = "Colorful rock face with diverse climbing techniques" },
                new Crux { Id = 13, Lat = "59.2150", Lng = "9.6135", Level = 4, Name = "Fossekammen", Description = "Technical climb alongside the ridge of a waterfall" },
                new Crux { Id = 14, Lat = "59.2155", Lng = "9.6140", Level = 2, Name = "Solstien", Description = "Sunny route perfect for afternoon climbing" },
                new Crux { Id = 15, Lat = "59.2160", Lng = "9.6145", Level = 5, Name = "Trollveggen Jr.", Description = "Mini version of Norway's famous Troll Wall challenge" },
                new Crux { Id = 16, Lat = "59.2165", Lng = "9.6150", Level = 1, Name = "Barneklatringen", Description = "Child-friendly route with easy access and safe holds" },
                new Crux { Id = 17, Lat = "59.2170", Lng = "9.6155", Level = 3, Name = "Måneskinnet", Description = "Beautiful climb especially popular for evening sessions" },
                new Crux { Id = 18, Lat = "59.2175", Lng = "9.6160", Level = 4, Name = "Nordlyset", Description = "Dynamic climbing with amazing northern views" },
                new Crux { Id = 19, Lat = "59.2180", Lng = "9.6165", Level = 2, Name = "Fjordveien", Description = "Gentle route with spectacular fjord views" },
                new Crux { Id = 20, Lat = "59.2185", Lng = "9.6170", Level = 5, Name = "Isbrekkeren", Description = "Challenging route with ice-like rock formations" },
                new Crux { Id = 21, Lat = "59.2190", Lng = "9.6175", Level = 1, Name = "Svaberget", Description = "Smooth slab climbing on polished granite" }
            };
        }
    }
}
