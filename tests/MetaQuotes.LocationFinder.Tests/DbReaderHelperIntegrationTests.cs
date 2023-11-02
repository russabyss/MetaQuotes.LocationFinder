using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Helpers;

namespace MetaQuotes.LocationFinder.Tests
{
    public class DbReaderHelperIntegrationTests
    {
        [Fact]
        public void GetHeader_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            var buffer = File.ReadAllBytes("Data/geobase.dat");

            // Act
            var header = DbReaderHelper.GetHeader(buffer);

            // Assert
            Assert.Equal("Geo.IP", header.Name);
            Assert.Equal<int>(1, header.Version);
            Assert.Equal<int>(100000, header.Records);
            Assert.Equal<ulong>(1487167858, header.Timestamp);
            Assert.Equal<uint>(10800060, header.OffsetCities);
            Assert.Equal<uint>(60, header.OffsetRanges);
            Assert.Equal<uint>(1200060, header.OffsetLocations);
        }        
        
        [Fact]
        public void GetIntervals_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            var buffer = File.ReadAllBytes("Data/geobase.dat");
            var header = DbReaderHelper.GetHeader(buffer);

            // Act
            var intervals = DbReaderHelper.GetIpIntervals(
                buffer
                    .AsSpan()
                    .Slice((int)header.OffsetRanges, header.Records * DbConstants.IpIntervalLength), // ƒл€ упрощени€ - делаем пр€мой каст uint -> int, т.к. в текущем файле точно все будет ок.
                header.Records);

            // Assert
            Assert.Equal(header.Records, intervals.Length);
            // TODO: проверить, что IpFrom <= IpTo во всех элементах.
        } 
        
        [Fact]
        public void GetLocations_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            var buffer = File.ReadAllBytes("Data/geobase.dat");
            var header = DbReaderHelper.GetHeader(buffer);

            // Act
            var locations = DbReaderHelper.GetLocations(
                buffer
                    .AsSpan()
                    .Slice((int)header.OffsetLocations, header.Records * DbConstants.LocationLength), // ƒл€ упрощени€ - делаем пр€мой каст uint -> int, т.к. в текущем файле точно все будет ок.
                header.Records);

            // Assert
            Assert.Equal(header.Records, locations.Length);
            // TODO: проверить, что все city начинаютс€ с cit, etc.
        }  
        
        [Fact]
        public void GetIpIndex_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            var buffer = File.ReadAllBytes("Data/geobase.dat");
            var header = DbReaderHelper.GetHeader(buffer);

            // Act
            var ipSearchIndex = DbReaderHelper.GetIpSearchIndex(
                buffer
                    .AsSpan()
                    .Slice((int)header.OffsetRanges, header.Records * DbConstants.IpIntervalLength), // ƒл€ упрощени€ - делаем пр€мой каст uint -> int, т.к. в текущем файле точно все будет ок.
                header.Records);

            // Assert
            Assert.Equal(header.Records, ipSearchIndex.IpsFrom.Length);
            Assert.Equal(header.Records, ipSearchIndex.IpsTo.Length);
            Assert.Equal(header.Records, ipSearchIndex.LocationIndexes.Length);
            // TODO: проверить, что все IpTo <= IpFrom на одинаковых индексах массива.
        }     
        
        [Fact]
        public void GetCitySearchIndex_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            var buffer = File.ReadAllBytes("Data/geobase.dat");
            var header = DbReaderHelper.GetHeader(buffer);
            var locations = buffer.AsMemory((int)header.OffsetLocations, DbConstants.LocationLength * header.Records);
            var citiesList = buffer.AsSpan((int)header.OffsetCities, header.Records * DbConstants.LocationsListItem);

            // Act
            var citySearchIndex = DbReaderHelper.GetCitySearchIndex(
                locations,
                citiesList,
                header.Records);

            // Assert
            Assert.Equal(header.Records, citySearchIndex.Cities.Length);
            // TODO: проверить, массив Cities - упор€дочен.
        }     
        
        [Fact]
        public void CreateSearchIndex_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            

            // Act
            var searchIndex = DbReaderHelper.CreateSearchIndex("Data/geobase.dat");

            // Assert
            Assert.Equal(searchIndex.Header.Records, searchIndex.CitySearchIndex.Cities.Length);
            Assert.Equal(searchIndex.Header.Records, searchIndex.IpSearchIndex.IpsTo.Length);
            Assert.Equal(searchIndex.Header.Records, searchIndex.IpSearchIndex.IpsFrom.Length);
            Assert.Equal(searchIndex.Header.Records, searchIndex.IpSearchIndex.LocationIndexes.Length);
        }
    }
}