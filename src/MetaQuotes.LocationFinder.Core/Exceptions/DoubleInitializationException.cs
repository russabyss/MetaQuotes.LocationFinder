namespace MetaQuotes.LocationFinder.Core.Exceptions
{
    /// <summary>
    /// Исключение при двойной инициализации.
    /// </summary>
    public class DoubleInitializationException : Exception
    {
        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public DoubleInitializationException(string message) : base(message)
        {
            
        }
    }
}
