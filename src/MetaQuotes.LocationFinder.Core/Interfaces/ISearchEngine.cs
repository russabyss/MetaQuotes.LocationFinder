using MetaQuotes.LocationFinder.Contracts;

namespace MetaQuotes.LocationFinder.Core.Interfaces
{
    /// <summary>
    /// Интерфейс поискового сервиса.
    /// </summary>
    public interface ISearchEngine
    {
        /// <summary>
        /// Инициализировать необходимые зависимости.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Найти локацию по IP-адресу.
        /// </summary>
        /// <param name="ip">IP-адрес.</param>
        /// <returns>Локация. См. <see cref="Location"/>.</returns>
        Location FindLocationByIp(string ip);

        /// <summary>
        /// Найти все локации по названию города с учетом регистра.
        /// </summary>
        /// <param name="city">Название города.</param>
        /// <returns>Массив локаций. См. <see cref="Location"/>.</returns>
        IEnumerable<Location> FindLocationsByCity(string city);
    }
}
