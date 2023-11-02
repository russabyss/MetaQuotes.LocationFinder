namespace MetaQuotes.LocationFinder.Core.Models
{
    /// <summary>
    /// Поисковый индекс по городам.
    /// Города упорядочены по названию (порядок считывается из исходного
    /// файла БД). Индекс города в массиве соответствует индексу элемента
    /// в массиве адресов локаций из исходного файла.
    /// Это означает, что если мы бинарным поиском найдем подходящую ячейку в массиве
    /// городов, то по номеру этой ячейки сможем прочитать адрес локации в исходном файле.
    /// </summary>
    public class CitySearchIndex
    {
        /// <summary>
        /// Упорядоченный массив городов в байтовом представлении.
        /// </summary>
        public ReadOnlyMemory<byte>[] Cities
        {
            get;
        }

        /// <summary>
        /// Адреса локаций в исходном массиве байтов (файле) относительно заданного смещения,
        /// упорядоченные по названиям городов.
        /// </summary>
        public int[] LocationAddresses
        {
            get;
        }

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="recordsCount">Количество записей.</param>
        public CitySearchIndex(int recordsCount)
        {
            Cities = new ReadOnlyMemory<byte>[recordsCount];
            LocationAddresses = new int[recordsCount];
        }
    }
}
