using MetaQuotes.LocationFinder.Contracts;

namespace MetaQuotes.LocationFinder.Core.Extensions
{
    /// <summary>
    /// Методы расширения для <see cref="Span"/>.
    /// </summary>
    public static class SpanExtensions
    {
        /// <summary>
        /// Удалить нечитаемые символы с конца.
        /// Удаляет символы конца строки \0 и пробелы.
        /// </summary>
        /// <param name="span">Набор байтов.</param>
        /// <returns>Усеченный набор байтов.</returns>
        public static ReadOnlySpan<byte> TrimEscapeBytesEnd(this ReadOnlySpan<byte> span)
        {
            return span
                .TrimEnd(DbConstants.EmptySymbolCode)
                .TrimEnd(DbConstants.SpaceSymbolCode);
        }
    }
}
