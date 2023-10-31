using MetaQuotes.LocationFinder.Contracts;
using System;
using System.Text;

namespace MetaQuotes.LocationFinder.Core.Helpers
{
    public static class DbReaderHelper
    {
        private const byte EscapeEmptySymbolCode = 0;
        private const byte EscapeSpaceSymbolCode = 23;

        /// <summary>
        /// Загрузить базу данных из файла.
        /// Парсит данные и получает полную структуру данных.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Модель базы данных <see cref="DataBase"/>.</returns>
        /// <exception cref="FileNotFoundException">Проверяет наличие файла с базой данных по указанному пути.</exception>
        public static DataBase LoadFromFile(string filePath = "Data/geobase.dat")
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"DB file not found in '{filePath}'.");
            }

            var fileBuffer = File.ReadAllBytes(filePath);
            var header = GetHeader(fileBuffer);
            var ipIntervals = GetIpIntervals(
                fileBuffer
                    .AsSpan()
                    .Slice((int)header.OffsetRanges, header.Records * DbConstants.IpIntervalLength), // Для упрощения - делаем прямой каст uint -> int, т.к. в текущем файле точно все будет ок.
                header.Records);

            throw new NotImplementedException();
            //var locations = GetLocations(fileBuffer, header.Records);

            //return new DataBase(header, ipIntervals, locations);
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
            
            for (var i = 0; i < buffer.Length; i += 12)
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
    }
}
