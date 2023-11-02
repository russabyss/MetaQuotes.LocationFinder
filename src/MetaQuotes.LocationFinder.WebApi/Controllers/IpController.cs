using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MetaQuotes.LocationFinder.WebApi.Controllers
{
    [ApiController]
    [Route("ip")]
    public class IpController : ControllerBase
    {
        private readonly ISearchEngine _searchEngine;
        private readonly ILogger<IpController> _logger;

        /// <summary>
        /// �������.
        /// </summary>
        /// <param name="searchEngine">��������� ������.</param>
        /// <param name="logger">�����.</param>
        /// <exception cref="ArgumentNullException">�������� ������������ �� �������������.</exception>
        public IpController(
            ISearchEngine searchEngine,
            ILogger<IpController> logger)
        {
            _searchEngine = searchEngine ?? throw new ArgumentNullException(nameof(searchEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// �������� ������� �� IP.
        /// </summary>
        /// <param name="ip">IP-�����.</param>
        /// <returns>�������. ��. <see cref="Location"/>.</returns>
        [HttpGet, Route("location")]
        public Location Get([FromQuery] string ip)
        {
            return _searchEngine.FindLocationByIp(ip);
        }
    }
}