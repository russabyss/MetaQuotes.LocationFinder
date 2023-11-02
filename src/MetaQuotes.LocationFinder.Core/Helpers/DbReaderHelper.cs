using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Extensions;
using MetaQuotes.LocationFinder.Core.Models;
using System;
using System.Text;

namespace MetaQuotes.LocationFinder.Core.Helpers
{
    public static class DbReaderHelper
    {
        private const byte EscapeEmptySymbolCode = 0;
        private const byte EscapeSpaceSymbolCode = 23;

        /// <summary>
        /// Прочитать файл с диска.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Байтовое представление файла.</returns>
        /// <exception cref="FileNotFoundException">Проверяет наличие файла с базой данных по указанному пути.</exception>
        public static byte[] ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"DB file not found in '{filePath}'.");
            }

            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Загрузить базу данных из файла.
        /// Парсит данные и получает полную структуру данных.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Модель базы данных <see cref="DataBase"/>.</returns>
        /// <exception cref="FileNotFoundException">Проверяет наличие файла с базой данных по указанному пути.</exception>
        public static DataBase LoadFromFile(string filePath)
        {
            var fileBuffer = ReadFile(filePath);
            
            var header = GetHeader(fileBuffer);
            
            var ipIntervals = GetIpIntervals(
                fileBuffer
                    .AsSpan()
                    .Slice((int)header.OffsetRanges, header.Records * DbConstants.IpIntervalLength), // Для упрощения - делаем прямой каст uint -> int, т.к. в текущем файле точно все будет ок.
                header.Records);
            
            var locations = GetLocations(
                fileBuffer
                    .AsSpan()
                    .Slice((int)header.OffsetLocations, header.Records * DbConstants.LocationLength), // Для упрощения - делаем прямой каст uint -> int, т.к. в текущем файле точно все будет ок.
                header.Records);
          
            return new DataBase(header, ipIntervals, locations);
        }

        /// <summary>
        /// Создать поисковый индекс на основе исходного файла БД.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Поисковый индекс. См. <see cref="SearchIndex"/>.</returns>
        public static SearchIndex CreateSearchIndex(string filePath)
        {
            var fileBuffer = ReadFile(filePath).AsMemory();
            var header = GetHeader(fileBuffer.Span);

            var ipIntervals = fileBuffer.Span.Slice(
                (int)header.OffsetRanges, 
                header.Records * DbConstants.IpIntervalLength);

            var ipSearchIndex = GetIpSearchIndex(
                ipIntervals,
                header.Records);

            var locations = fileBuffer.Slice(
                (int)header.OffsetLocations, 
                DbConstants.LocationLength * header.Records);
            
            var citiesList = fileBuffer.Span.Slice(
                (int)header.OffsetCities,
                DbConstants.LocationsListItem * header.Records);

            var citySearchIndex = GetCitySearchIndex(
                locations, 
                citiesList, 
                header.Records);

            return new SearchIndex(fileBuffer, citySearchIndex, ipSearchIndex, header);
        }


        /// <summary>
        /// Распарсить заголовок базы данных.
        /// </summary>
        /// <param name="buffer">Массив байтов.</param>
        /// <returns>Заголовок базы данных - см. <see cref="Header"/>.</returns>
        internal static Header GetHeader(ReadOnlySpan<byte> buffer)
        {
            var currentPosition = 0;
            var version = BitConverter.ToInt32(buffer.Slice(currentPosition, DbConstants.IntLength));
            var name = Encoding.UTF8.GetString(buffer.Slice(currentPosition += DbConstants.IntLength, DbConstants.HeaderNameLength)
                .TrimEnd(EscapeEmptySymbolCode));
            var timestamp = BitConverter.ToUInt64(buffer.Slice(currentPosition += DbConstants.HeaderNameLength, DbConstants.UlongLength));
            var records = BitConverter.ToInt32(buffer.Slice(currentPosition += DbConstants.UlongLength, DbConstants.IntLength));
            var offsetRanges = BitConverter.ToUInt32(buffer.Slice(currentPosition += DbConstants.IntLength, DbConstants.UintLength));
            var offsetCities = BitConverter.ToUInt32(buffer.Slice(currentPosition += DbConstants.UintLength, DbConstants.UintLength));
            var offsetLocations = BitConverter.ToUInt32(buffer.Slice(currentPosition += DbConstants.UintLength, DbConstants.UintLength));

            return new Header(
                version,
                name,
                timestamp,
                records,
                offsetRanges,
                offsetCities,
                offsetLocations
            );
        }

        /// <summary>
        /// Распарсить интервалы ip-адресов.
        /// </summary>
        /// <param name="buffer">Исходный массив байтов.</param>
        /// <param name="recordsCount">Количество записей.</param>
        /// <returns>Массив интервалов ip-адресов. См. <see cref="IpInterval"/>.</returns>
        internal static IpInterval[] GetIpIntervals(ReadOnlySpan<byte> buffer, int recordsCount)
        {
            var ipIntervals = new IpInterval[recordsCount];
            var currentIntervalIndex = 0;
            
            for (var i = 0; i < buffer.Length; i += DbConstants.IpIntervalLength)
            {
                var startIndex = i;
                var ipFrom = BitConverter.ToUInt32(buffer.Slice(i, DbConstants.UintLength));
                var ipTo = BitConverter.ToUInt32(buffer.Slice(startIndex += DbConstants.UintLength, DbConstants.UintLength));
                var locationIndex = BitConverter.ToUInt32(buffer.Slice(startIndex += DbConstants.UintLength, DbConstants.UintLength));

                ipIntervals[currentIntervalIndex] = new IpInterval(ipFrom, ipTo, locationIndex);

                currentIntervalIndex++;
            }

            return ipIntervals;
        }


        /// <summary>
        /// Распарсить все локации.
        /// </summary>
        /// <param name="buffer">Исходный массив байтов.</param>
        /// <param name="recordsCount">Количество записей.</param>
        /// <returns>Массив локаций. См. <see cref="Location"/>.</returns>
        internal static Location[] GetLocations(ReadOnlySpan<byte> buffer, int recordsCount)
        {
            var locations = new Location[recordsCount];
            var currentLocationIndex = 0;
            
            for (var i = 0; i < buffer.Length; i += DbConstants.LocationLength)
            {
                var locationsSpan = buffer.Slice(i, DbConstants.LocationLength);
                var location = GetLocation(locationsSpan);

                locations[currentLocationIndex] = location;
                currentLocationIndex++;
            }

            return locations;
        }

        /// <summary>
        /// Распарсить локацию.
        /// </summary>
        /// <param name="locationsSpan">Исходный набор байтов - только байты локации, без лишней информации.</param>
        /// <returns>Локация. См. <see cref="Location"/>.</returns>
        internal static Location GetLocation(ReadOnlySpan<byte> buffer)
        {
            var currentPosition = 0;
            var country = Encoding.UTF8.GetString(buffer.Slice(currentPosition, DbConstants.LocationCountryLength).TrimEnd(EscapeEmptySymbolCode));
            var region = Encoding.UTF8.GetString(buffer.Slice(currentPosition += DbConstants.LocationCountryLength, DbConstants.LocationRegionLength).TrimEnd(EscapeEmptySymbolCode));
            var postal = Encoding.UTF8.GetString(buffer.Slice(currentPosition += DbConstants.LocationRegionLength, DbConstants.LocationPostalLength).TrimEnd(EscapeEmptySymbolCode));
            var city = Encoding.UTF8.GetString(buffer.Slice(currentPosition += DbConstants.LocationPostalLength, DbConstants.LocationCityLength).TrimEnd(EscapeEmptySymbolCode));
            var org = Encoding.UTF8.GetString(buffer.Slice(currentPosition += DbConstants.LocationCityLength, DbConstants.LocationOrganizationLength).TrimEnd(EscapeEmptySymbolCode));
            var latitude = BitConverter.ToSingle(buffer.Slice(currentPosition += DbConstants.LocationOrganizationLength, DbConstants.FloatLength));
            var longitude = BitConverter.ToSingle(buffer.Slice(currentPosition += DbConstants.FloatLength, DbConstants.FloatLength));

            return new Location
            (
                country,
                region,
                postal,
                city,
                org,
                latitude,
                longitude
            );
        }

        /// <summary>
        /// Распарсить исходный набор байтов в поисковый индекс
        /// по IP-адресам.
        /// </summary>
        /// <param name="intervalsBuffer">Исходный набор байтов.</param>
        /// <param name="recordsCount">Количество записей.</param>
        /// <returns>Поисковый индекс по IP-адресам. См. <see cref="IpSearchIndex"/>.</returns>
        internal static IpSearchIndex GetIpSearchIndex(
            ReadOnlySpan<byte> intervalsBuffer, 
            int recordsCount)
        {
            var ipIndex = new IpSearchIndex(recordsCount);
            var currentIndex = 0;
            for (var i = 0; i < intervalsBuffer.Length; i += DbConstants.IpIntervalLength)
            {
                var currentPosition = i;
                var ipFrom = BitConverter.ToUInt32(intervalsBuffer.Slice(currentPosition, DbConstants.UintLength));
                var ipTo = BitConverter.ToUInt32(intervalsBuffer.Slice(currentPosition += DbConstants.UintLength, DbConstants.UintLength));
                var locationIndex = BitConverter.ToUInt32(intervalsBuffer.Slice(currentPosition += DbConstants.UintLength, DbConstants.UintLength));

                ipIndex.Add(ipFrom, ipTo, locationIndex, currentIndex);

                currentIndex++;
            }

            return ipIndex;
        }

        /// <summary>
        /// Распарсить исходный набор байтов в поисковый индекс
        /// по городам.
        /// Считывает адреса локаций из списка упорядоченных по названию городов адресов,
        /// по найденным адресам считывает наборы байтов, соответствующие названиям 
        /// городов и записывает их в установленном порядке в поисковый индекс.
        /// </summary>
        /// <param name="locationsbuffer">Исходный набор байтов для локаций.</param>
        /// <param name="citiesListBuffer">Исходный набор байтов для упорядоченного списка адресов локаций.</param>
        /// <param name="recordCount">Количество записей.</param>
        /// <returns></returns>
        internal static CitySearchIndex GetCitySearchIndex(
            ReadOnlyMemory<byte> locationsbuffer, 
            ReadOnlySpan<byte> citiesListBuffer, 
            int recordCount)
        {
            var citySearchIndex = new CitySearchIndex(recordCount);
            var locationToCityOffset = DbConstants.LocationCountryLength + DbConstants.LocationRegionLength + DbConstants.LocationPostalLength;

            var currentIndex = 0;
            for (var i = 0; i < citiesListBuffer.Length; i += DbConstants.LocationsListItem)
            {
                var locationAddress = BitConverter.ToInt32(citiesListBuffer.Slice(i, DbConstants.IntLength));
                var cityName = locationsbuffer.Slice(locationAddress + locationToCityOffset, DbConstants.LocationCityLength);

                citySearchIndex.Add(cityName, currentIndex);
                currentIndex++;
            }

            return citySearchIndex;
        }
    }
}
