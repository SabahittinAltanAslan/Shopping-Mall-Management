using Iyaspark.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Iyaspark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumsController : ControllerBase
    {
        [HttpGet("sectors")]
        public IActionResult GetSectors()
        {
            var sectors = Enum.GetValues(typeof(Sector))
                              .Cast<Sector>()
                              .Select(e => new
                              {
                                  key = e,
                                  value = e.ToString()
                              });
            return Ok(sectors);
        }
    }
}
