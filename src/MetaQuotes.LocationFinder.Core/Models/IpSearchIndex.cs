namespace MetaQuotes.LocationFinder.Core.Models
{
    /// <summary>
    /// Поисковый индекс по IP-адресам.
    /// Все внутренние массивы синхронизированы по длине.
    /// </summary>
    public class IpSearchIndex
    {
        /// <summary>
        /// Массив начальных значений интервалов адресов.
        /// </summary>
        public uint[] IpsFrom { get; }

        /// <summary>
        /// Массив конечных значений интервалов адресов.
        /// </summary>
        public uint[] IpsTo { get; }

        /// <summary>
        /// Массив индексов локаций в массиве локаций.
        /// </summary>
        public uint[] LocationIndexes { get; }

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="recordsCount">Количество записей.</param>
        public IpSearchIndex(int recordsCount)
        {
            IpsFrom = new uint[recordsCount];
            IpsTo = new uint[recordsCount];
            LocationIndexes = new uint[recordsCount];
        }
    }
}
