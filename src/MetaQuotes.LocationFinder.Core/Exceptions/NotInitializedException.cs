namespace MetaQuotes.LocationFinder.Core.Exceptions
{
    /// <summary>
    /// Исключение при вызове бизнес-логики до завершения инициализации.
    /// </summary>
    public class NotInitializedException : Exception
    {
        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public NotInitializedException(string message) : base(message)
        {

        }
    }
}
