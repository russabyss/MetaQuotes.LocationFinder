using MetaQuotes.LocationFinder.Core.Models;

namespace MetaQuotes.LocationFinder.Core.Extensions
{
    /// <summary>
    /// Методы расширения для <see cref="CitySearchIndex"/>.
    /// </summary>
    public static class CitySearchIndexExtensions
    {
        /// <summary>
        /// Добавить значение в поисковый индекс.
        /// </summary>
        /// <param name="searchIndex">Поисковый индекс.</param>
        /// <param name="city">Добавляемый город.</param>
        /// <param name="index">Номер ячейки для записи значения.</param>
        /// <exception cref="IndexOutOfRangeException">Проверка номера ячейки на соответствие интервалу.</exception>
        public static void Add(
            this CitySearchIndex searchIndex, 
            ReadOnlyMemory<byte> city,
            int index)
        {
            if (index >= searchIndex.Cities.Length || index < 0)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            searchIndex.Cities[index] = city;
        }
    }
}
