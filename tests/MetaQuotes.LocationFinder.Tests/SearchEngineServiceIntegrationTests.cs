using MetaQuotes.LocationFinder.Core.Helpers;
using MetaQuotes.LocationFinder.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace MetaQuotes.LocationFinder.Tests
{
    public class SearchEngineServiceIntegrationTests : IClassFixture<SearchEngineFixture>
    {
        private SearchEngineService _searchEngineService;

        public SearchEngineServiceIntegrationTests(SearchEngineFixture searchEngineFixture)
        {
            _searchEngineService = searchEngineFixture.SearchEngine;
        }

        [Fact]
        public void FindLocationByIp_OrdinalSearch_ReturnsExpected()
        {
            // Arrange
            var ip = "";

            // Act
            var location = _searchEngineService.FindLocationByIp(ip);

            // Assert
            Assert.Equal(null, location);
        }

        [Fact]
        public void FindLocationByIp_BadValue_ThrowsExpected()
        {
            // Arrange
            var ip = "";

            // Assert
            Assert.Throws<Exception>(() => { _searchEngineService.FindLocationByIp(ip); });
        }

    }

    public class SearchEngineFixture
    {
        public SearchEngineService SearchEngine { get; }

        public SearchEngineFixture()
        {
            var stubLogger = Mock.Of<ILogger<SearchEngineService>>();
            var searchIndex = DbReaderHelper.CreateSearchIndex("Data/geobase.dat");

            SearchEngine = new SearchEngineService(
                searchIndex,
                stubLogger);
        }
    }
}
