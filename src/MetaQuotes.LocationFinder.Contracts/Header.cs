namespace MetaQuotes.LocationFinder.Contracts
{
    /// <summary>
    /// Заголовок базы данных.
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Версия базы данных.
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Название/префикс для базы данных.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Время создания базы данных.
        /// </summary>
        public ulong Timestamp { get; }

        /// <summary>
        /// Общее количество записей.
        /// </summary>
        public int Records { get; }

        /// <summary>
        /// Смещение относительно начала файла до начала списка записей с геоинформацией.
        /// </summary>
        public uint OffsetRanges { get; }

        /// <summary>
        /// Смещение относительно начала файла до начала индекса с сортировкой по названию городов.
        /// </summary>
        public uint OffsetCities { get; }

        /// <summary>
        /// Смещение относительно начала файла до начала списка записей о местоположении.
        /// </summary>
        public uint OffsetLocations { get; }

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="version">Версия базы данных.</param>
        /// <param name="name">Название/префикс для базы данных.</param>
        /// <param name="timestamp">Время создания базы данных.</param>
        /// <param name="records">Общее количество записей.</param>
        /// <param name="offsetRanges">Смещение относительно начала файла до начала списка записей с геоинформацией.</param>
        /// <param name="offsetCities">Смещение относительно начала файла до начала индекса с сортировкой по названию городов.</param>
        /// <param name="offsetLocations">Смещение относительно начала файла до начала списка записей о местоположении.</param>
        public Header(
            int version, 
            string name, 
            ulong timestamp, 
            int records, 
            uint offsetRanges, 
            uint offsetCities, 
            uint offsetLocations)
        {
            Version = version;
            Name = name;
            Timestamp = timestamp;
            Records = records;
            OffsetRanges = offsetRanges;
            OffsetCities = offsetCities;
            OffsetLocations = offsetLocations;
        }
    }
}
