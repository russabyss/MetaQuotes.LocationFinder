using MetaQuotes.LocationFinder.Contracts;

namespace MetaQuotes.LocationFinder.Core.Models
{
    /// <summary>
    /// Поисковый индекс.
    /// </summary>
    public class SearchIndex
    {
        /// <summary>
        /// Заголовок БД.
        /// </summary>
        public Header Header { get; }

        /// <summary>
        /// Исходный массив байтов из файла БД.
        /// </summary>
        public ReadOnlyMemory<byte> OriginBytes { get; }

        public ReadOnlySpan<byte> LocationsBytes => OriginBytes.Span.Slice(
            (int)Header.OffsetLocations, 
            Header.Records * DbConstants.LocationLength);

        /// <summary>
        /// Поисковый индекс по названиям городов.
        /// </summary>
        public CitySearchIndex CitySearchIndex { get; }

        /// <summary>
        /// Поисковый индекс по интервалам IP-адресов.
        /// </summary>
        public IpSearchIndex IpSearchIndex { get; }

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="originBytes">Исходный массив байтов из файла БД.</param>
        /// <param name="citySearchIndex">Поисковый индекс по названиям городов.</param>
        /// <param name="ipSearchIndex">Поисковый индекс по интервалам IP-адресов.</param>
        /// <exception cref="ArgumentNullException">Проверка параметров на существование.</exception>
        public SearchIndex(
            ReadOnlyMemory<byte> originBytes,
            CitySearchIndex citySearchIndex,
            IpSearchIndex ipSearchIndex,
            Header header)
        {
            OriginBytes = originBytes;
            CitySearchIndex = citySearchIndex ?? throw new ArgumentNullException(nameof(citySearchIndex));
            IpSearchIndex = ipSearchIndex ?? throw new ArgumentNullException(nameof(ipSearchIndex));
            Header = header ?? throw new ArgumentNullException(nameof(header));
        }
    }
}
