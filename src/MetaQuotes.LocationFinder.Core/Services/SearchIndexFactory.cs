using MetaQuotes.LocationFinder.Core.Helpers;
using MetaQuotes.LocationFinder.Core.Models;
using MetaQuotes.LocationFinder.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MetaQuotes.LocationFinder.Core.Services
{
    /// <summary>
    /// Фабрика для поискового индекса.
    /// </summary>
    public class SearchIndexFactory
    {
        private readonly IOptions<DbSettings> _options;
        private readonly ILogger<SearchIndexFactory> _logger;

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="options">Настройки.</param>
        /// <param name="logger">Логер.</param>
        public SearchIndexFactory(
            IOptions<DbSettings> options, 
            ILogger<SearchIndexFactory> logger)
        {
            _options = options;
            _logger = logger;
        }

        /// <summary>
        /// Создать индекс.
        /// </summary>
        /// <returns>Поисковый индекс. См. <see cref="SearchIndex"/>.</returns>
        public SearchIndex CreateIndex()
        {
            _logger.LogDebug("Начинаем чтение БД из файла `{FilePath}`", _options.Value.FilePath);

            var index = DbReaderHelper.CreateSearchIndex(_options.Value.FilePath);

            _logger.LogDebug("Чтение БД из файла завершено.");

            return index;
        }
    }
}
