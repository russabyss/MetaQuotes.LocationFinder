namespace MetaQuotes.LocationFinder.Contracts
{
    /// <summary>
    /// Локация.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Название страны (случайная строка с префиксом "cou_").
        /// </summary>
        public string Country
        {
            get;
        }

        /// <summary>
        /// Название области (случайная строка с префиксом "reg_").
        /// </summary>
        public string Region
        {
            get;
        }

        /// <summary>
        /// Почтовый индекс (случайная строка с префиксом "pos_").
        /// </summary>
        public string Postal
        {
            get;
        }

        /// <summary>
        /// Название города (случайная строка с префиксом "cit_").
        /// </summary>
        public string City
        {
            get;
        }

        /// <summary>
        /// Название организации (случайная строка с префиксом "org_").
        /// </summary>
        public string Organization
        {
            get;
        }

        /// <summary>
        /// Широта.
        /// </summary>
        public float Latitude
        {
            get;
        }

        /// <summary>
        /// Долгота.
        /// </summary>
        public float Longitude
        {
            get;
        }

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="country">Название страны (случайная строка с префиксом "cou_").</param>
        /// <param name="region">Название области (случайная строка с префиксом "reg_").</param>
        /// <param name="postal">Почтовый индекс (случайная строка с префиксом "pos_").</param>
        /// <param name="city">Название города (случайная строка с префиксом "cit_").</param>
        /// <param name="organization">Название организации (случайная строка с префиксом "org_").</param>
        /// <param name="latitude">Широта.</param>
        /// <param name="longitude">Долгота.</param>
        public Location(
            string country, 
            string region, 
            string postal, 
            string city, 
            string organization, 
            float latitude, 
            float longitude)
        {
            Country = country;
            Region = region;
            Postal = postal;
            City = city;
            Organization = organization;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}