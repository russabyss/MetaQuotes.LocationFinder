using MetaQuotes.LocationFinder.Core.Models;

namespace MetaQuotes.LocationFinder.Core.Extensions
{
    /// <summary>
    /// Методы расширения для <see cref="IpSearchIndex"/>.
    /// </summary>
    public static class IpSearchIndexExtensions
    {
        /// <summary>
        /// Добавить значение в поисковый индекс.
        /// </summary>
        /// <param name="searchIndex">Поисковы индекс.</param>
        /// <param name="ipFrom">Начальное значение интервала IP-адресов.</param>
        /// <param name="ipTo">Конечное значение интервала IP-адресов.</param>
        /// <param name="locationIndex">Индекс локации.</param>
        /// <param name="index">Номер ячейки, в которую необходимо добавить значение.</param>
        /// <exception cref="IndexOutOfRangeException">Проверка номера ячейки на соответствие интервалу.</exception>
        public static void Add(
            this IpSearchIndex searchIndex, 
            uint ipFrom, 
            uint ipTo, 
            uint locationIndex, 
            int index)
        {
            if(index >= searchIndex.IpsTo.Length ||  index < 0)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            searchIndex.IpsFrom[index] = ipFrom;
            searchIndex.IpsTo[index] = ipTo;
            searchIndex.LocationIndexes[index] = locationIndex;
        }
    }
}
