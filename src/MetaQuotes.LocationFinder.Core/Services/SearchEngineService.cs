using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Extensions;
using MetaQuotes.LocationFinder.Core.Interfaces;
using MetaQuotes.LocationFinder.Core.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MetaQuotes.LocationFinder.Core.Services;

/// <summary>
/// Сервис поиска локаций.
/// </summary>
public class SearchEngineService : ISearchEngine
{
    private readonly SearchIndex _searchIndex;

    private readonly ILogger<SearchEngineService> _logger;

    /// <summary>
    /// Создать.
    /// </summary>
    /// <param name="logger">Логер.</param>
    public SearchEngineService(
        SearchIndex searchIndex,
        ILogger<SearchEngineService> logger)
    {
        _searchIndex = searchIndex ?? throw new ArgumentNullException(nameof(searchIndex));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public Location FindLocationByIp(string ip)
    {
        var ipAddress = IPAddress.Parse(ip);
        var ipAddressValue = BitConverter.ToUInt32(ipAddress.GetAddressBytes());
        var indexOfInterval = FindIpIndex(ipAddressValue);
        var location = _searchIndex.GetLocationByArrayIndex(indexOfInterval);

        return location;
    }

    /// <inheritdoc />
    public Location[] FindLocationsByCity(string city)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Бинарный поиск по массиву упорядоченных IP-адресов,
    /// представляющих собой начала интервалов.
    /// Находим ближайшее значение слева (номер ячейки),
    /// затем с помощью номера ячейки получаем значение справа (конец интервала)
    /// и проверяем, входит ли значени в этот интервал.
    /// </summary>
    /// <param name="ip">IP-адрес, по которому осуществляется поиск.</param>
    /// <returns>Номер ячейки в массиве IP-адресов, соответствующий интервалу входного адреса.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Проверка на вхождение в допустимый интервал.</exception>
    private int FindIpIndex(uint ip)
    {
        // Т.к. все интервалы IP-адресов упорядочены по возрастанию, проверяем
        // не выходит ли искомый адрес за пределы допустимого интервала.
        if (ip > _searchIndex.IpSearchIndex.IpsTo[_searchIndex.IpSearchIndex.IpsTo.Length - 1] ||
            ip < _searchIndex.IpSearchIndex.IpsFrom[0])
        {
            throw new ArgumentOutOfRangeException(nameof(ip));
        }

        var from = 0;
        var to = _searchIndex.IpSearchIndex.IpsFrom.Length - 1;
        var initialIndex = _searchIndex.IpSearchIndex.IpsFrom.Length / 2;
        var currentElement = _searchIndex.IpSearchIndex.IpsFrom[initialIndex];

        while (from <= to)
        {
            if (ip > currentElement)
            {
                from = initialIndex + 1;
            }
            else
            {
                to = initialIndex - 1;
            }

            initialIndex = (from + to) / 2;
            currentElement = _searchIndex.IpSearchIndex.IpsFrom[initialIndex];
        }

        var ipFromValue = _searchIndex.IpSearchIndex.IpsFrom[from];
        var ipToValue = _searchIndex.IpSearchIndex.IpsTo[from];

        if (ipFromValue <= ip && ip <= ipToValue)
        {
            return from;
        }

        ipFromValue = _searchIndex.IpSearchIndex.IpsFrom[to];
        ipToValue = _searchIndex.IpSearchIndex.IpsTo[to];

        if (ipFromValue <= ip && ip <= ipToValue)
        {
            return to;
        }

        throw new ArgumentOutOfRangeException(nameof(ip));
    }
}
