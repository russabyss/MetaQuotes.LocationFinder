using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MetaQuotes.LocationFinder.WebApi.Controllers
{
    [ApiController]
    [Route("city")]
    public class CityController : ControllerBase
    {
        private readonly ISearchEngine _searchEngine;
        private readonly ILogger<IpController> _logger;

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="searchEngine">Поисковый движок.</param>
        /// <param name="logger">Логер.</param>
        /// <exception cref="ArgumentNullException">Проверка зависимостей на существование.</exception>
        public CityController(
            ISearchEngine searchEngine,
            ILogger<IpController> logger)
        {
            _searchEngine = searchEngine ?? throw new ArgumentNullException(nameof(searchEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получить локацию по названию города.
        /// </summary>
        /// <param name="city">Название города.</param>
        /// <returns>Локации. См. <see cref="Location"/>.</returns>
        [HttpGet(Name = "locations")]
        public IEnumerable<Location> Get([FromQuery] string city)
        {
            return _searchEngine.FindLocationsByCity(city);
        }
    }
}