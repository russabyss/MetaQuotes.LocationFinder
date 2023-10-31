using MetaQuotes.LocationFinder.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MetaQuotes.LocationFinder.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IpController : ControllerBase
    {
        private readonly ILogger<IpController> _logger;

        public IpController(ILogger<IpController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "location")]
        public Location Get([FromQuery] string ip)
        {
            throw new NotImplementedException();
        }
    }
}