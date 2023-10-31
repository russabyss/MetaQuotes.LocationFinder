using System.Net;

namespace MetaQuotes.LocationFinder.Contracts
{
    /// <summary>
    /// Интервал IP-адресов.
    /// </summary>
    public class IpInterval
    {
        /// <summary>
        /// Начало диапазона IP адресов.
        /// </summary>
        public IPAddress IpFrom { get; }

        /// <summary>
        /// Конец диапазона IP адресов.
        /// </summary>
        public IPAddress IpTo { get; }

        /// <summary>
        /// Индекс записи о местоположении.
        /// </summary>
        public uint LocationIndex { get; }

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="ipFrom">Начало диапазона IP адресов.</param>
        /// <param name="ipTo">Конец диапазона IP адресов.</param>
        /// <param name="locationIndex">Индекс записи о местоположении.</param>
        public IpInterval(
            uint ipFrom, 
            uint ipTo, 
            uint locationIndex)
        {
            IpFrom = new IPAddress(ipFrom);
            IpTo = new IPAddress(ipTo);
            LocationIndex = locationIndex;
        }
    }
}
