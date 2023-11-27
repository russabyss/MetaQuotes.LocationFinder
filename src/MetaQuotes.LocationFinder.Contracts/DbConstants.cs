namespace MetaQuotes.LocationFinder.Contracts
{
    /// <summary>
    /// Длины полей в файле базы данных.
    /// </summary>
    public class DbConstants
    {
        /// <summary>
        /// Байт символа конца строки \0.
        /// </summary>
        public const byte EmptySymbolCode = 0;
        
        /// <summary>
        /// Байт пробельного символа.
        /// </summary>
        public const byte SpaceSymbolCode = 32;

        /// <summary>
        /// Размер заголовка в байтах.
        /// </summary>
        public const int HeaderLength = 60;

        /// <summary>
        /// Размер поля Name в заголовке в байтах.
        /// </summary>
        public const int HeaderNameLength = 32;

        /// <summary>
        /// Размер int поля в байтах.
        /// </summary>
        public const int IntLength = 4;

        /// <summary>
        /// Размер ulong поля в байтах.
        /// </summary>
        public const int UlongLength = 8;

        /// <summary>
        /// Размер uint поля в байтах.
        /// </summary>
        public const int UintLength = 4;

        /// <summary>
        /// Размер float поля в байтах.
        /// </summary>
        public const int FloatLength = 4;

        /// <summary>
        /// Размер одной записи с интервалом ip-адресов в байтах.
        /// </summary>
        public const int IpIntervalLength = 12;

        /// <summary>
        /// Размер одной записи о локации в байтах.
        /// </summary>
        public const int LocationLength = 96;

        /// <summary>
        /// Размер названия страны в байтах.
        /// </summary>
        public const int LocationCountryLength = 8;

        /// <summary>
        /// Размер названия региона в байтах.
        /// </summary>
        public const int LocationRegionLength = 12;

        /// <summary>
        /// Размер почтового индекса в байтах.
        /// </summary>
        public const int LocationPostalLength = 12;

        /// <summary>
        /// Размер названия города в байтах.
        /// </summary>
        public const int LocationCityLength = 24;

        /// <summary>
        /// Размер названия организации в байтах.
        /// </summary>
        public const int LocationOrganizationLength = 32;

        /// <summary>
        /// Размер одной записи в списке адресов локаций в байтах.
        /// </summary>
        public const int LocationsListItem = 4;
    }
}
