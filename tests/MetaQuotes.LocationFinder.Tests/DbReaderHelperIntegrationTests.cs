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
                    .Slice((int)header.OffsetRanges, header.Records * DbConstants.IpIntervalLength), // ��� ��������� - ������ ������ ���� uint -> int, �.�. � ������� ����� ����� ��� ����� ��.
                header.Records);

            // Assert
            Assert.Equal(header.Records, intervals.Length);
            // TODO: ���������, ��� IpFrom <= IpTo �� ���� ���������.
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
                    .Slice((int)header.OffsetLocations, header.Records * DbConstants.LocationLength), // ��� ��������� - ������ ������ ���� uint -> int, �.�. � ������� ����� ����� ��� ����� ��.
                header.Records);

            // Assert
            Assert.Equal(header.Records, locations.Length);
            // TODO: ���������, ��� ��� city ���������� � cit, etc.
        }
    }
}