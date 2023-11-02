using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaQuotes.LocationFinder.Core.Services;

/// <summary>
/// Сервис поиска локаций.
/// </summary>
public class SearchEngineService : ISearchEngine
{
    private readonly ILogger<SearchEngineService> _logger;

    /// <summary>
    /// Создать.
    /// </summary>
    /// <param name="logger">Логер.</param>
    public SearchEngineService(
        ILogger<SearchEngineService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Location FindLocationByIp(string ip)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Location[] FindLocationsByCity(string city)
    {
        throw new NotImplementedException();
    }
}
