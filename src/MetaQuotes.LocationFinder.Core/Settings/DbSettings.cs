namespace MetaQuotes.LocationFinder.Core.Settings
{
    /// <summary>
    /// Настройки базы данных.
    /// </summary>
    public class DbSettings
    {
        /// <summary>
        /// Название секции.
        /// </summary>
        public const string SectionName = "DbSettings";

        /// <summary>
        /// Путь к файлу.
        /// </summary>
        public string FilePath { get; set; }
    }
}
