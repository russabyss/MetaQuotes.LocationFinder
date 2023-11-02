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
        /// Создать.
        /// </summary>
        /// <param name="searchEngine">Поисковый движок.</param>
        /// <param name="logger">Логер.</param>
        /// <exception cref="ArgumentNullException">Проверка зависимостей на существование.</exception>
        public IpController(
            ISearchEngine searchEngine,
            ILogger<IpController> logger)
        {
            _searchEngine = searchEngine ?? throw new ArgumentNullException(nameof(searchEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получить локацию по IP.
        /// </summary>
        /// <param name="ip">IP-адрес.</param>
        /// <returns>Локация. См. <see cref="Location"/>.</returns>
        [HttpGet, Route("location")]
        public Location Get([FromQuery] string ip)
        {
            return _searchEngine.FindLocationByIp(ip);
        }
    }
}