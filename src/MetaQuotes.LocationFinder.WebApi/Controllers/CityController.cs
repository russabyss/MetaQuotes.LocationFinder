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
        /// �������.
        /// </summary>
        /// <param name="searchEngine">��������� ������.</param>
        /// <param name="logger">�����.</param>
        /// <exception cref="ArgumentNullException">�������� ������������ �� �������������.</exception>
        public CityController(
            ISearchEngine searchEngine,
            ILogger<IpController> logger)
        {
            _searchEngine = searchEngine ?? throw new ArgumentNullException(nameof(searchEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// �������� ������� �� �������� ������.
        /// </summary>
        /// <param name="city">�������� ������.</param>
        /// <returns>�������. ��. <see cref="Location"/>.</returns>
        [HttpGet(Name = "locations")]
        public IEnumerable<Location> Get([FromQuery] string city)
        {
            return _searchEngine.FindLocationsByCity(city);
        }
    }
}