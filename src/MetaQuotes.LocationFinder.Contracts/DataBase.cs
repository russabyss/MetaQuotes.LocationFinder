namespace MetaQuotes.LocationFinder.Contracts
{
    /// <summary>
    /// Модель базы данных.
    /// </summary>
    public class DataBase
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        public Header Header { get; }

        /// <summary>
        /// Интервалы адресов.
        /// </summary>
        public IpInterval[] IpIntervals { get; }

        /// <summary>
        /// Локации.
        /// </summary>
        public Location[] Locations { get; }

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="header">Заголовок.</param>
        /// <param name="ipIntervals">Интервалы ip-адресов.</param>
        /// <param name="locations">Локации.</param>
        public DataBase(
            Header header, 
            IpInterval[] ipIntervals, 
            Location[] locations)
        {
            Header = header;
            IpIntervals = ipIntervals;
            Locations = locations;
        }
    }
}
