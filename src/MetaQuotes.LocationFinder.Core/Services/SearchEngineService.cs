using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Extensions;
using MetaQuotes.LocationFinder.Core.Interfaces;
using MetaQuotes.LocationFinder.Core.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

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
    public IEnumerable<Location> FindLocationsByCity(string city)
    {
        if(string.IsNullOrEmpty(city))
        {
            throw new ArgumentNullException(nameof(city));
        }

        var cityBytes = Encoding.UTF8.GetBytes(city)
            .AsSpan()
            .TrimEnd(DbConstants.SpaceSymbolCode);
        
        var locationsAddresses = FindLocationsAddresses(cityBytes);
        var locations = new List<Location>(locationsAddresses.Count);

        foreach (var index in locationsAddresses)
        {
            var location = _searchIndex.GetLocationByAddressInFile(index);
            locations.Add(location);
        }

        return locations;
    }

    /// <summary>
    /// Бинарный поиск по названию города.
    /// Названия неуникальны, поэтому возвращает список идентификаторов
    /// локаций (адресов в исходном файле относительно смещения).
    /// </summary>
    /// <param name="cityBytes">Байтовое представление названия города.</param>
    /// <returns>Список адресов локаций из исходного файла относительно заданного смещения.</returns>
    private List<int> FindLocationsAddresses(ReadOnlySpan<byte> cityBytes)
    {
        var locationIndexes = new List<int>();
        var from = 0;
        var to = _searchIndex.CitySearchIndex.Cities.Length - 1;
        var initialIndex = _searchIndex.CitySearchIndex.Cities.Length / 2;
        var currentElement = _searchIndex.CitySearchIndex.Cities[initialIndex];
        var found = false;

        while (from <= to)
        {
            var currentElementTrimmed = currentElement.Span.TrimEscapeBytesEnd();
            
            var compareResult = cityBytes.SequenceCompareTo(currentElementTrimmed);

            if (compareResult > 0)
            {
                from = initialIndex + 1;
            }
            else if (compareResult < 0)
            {
                to = initialIndex - 1;
            }
            else if (compareResult == 0)
            {
                found = true;
                break;
            }

            initialIndex = (from + to) / 2;
            currentElement = _searchIndex.CitySearchIndex.Cities[initialIndex];
        }

        if (!found)
        {
            return locationIndexes;
        }

        // Бинарным поиском мы попали в рандомную точку интервала, состоящего из одинаковых названий городов.
        // Слева и справа от этой точки могут находиться другие подходящие элементы.
        // Необходимо пройтись налево и направо и дозаполнить список идентификаторов подходящими значениями.
        var foundIndexDown = initialIndex - 1;
        var currentElementDown = _searchIndex.CitySearchIndex.Cities[foundIndexDown].Span.TrimEscapeBytesEnd();

        while (foundIndexDown >= 0 && cityBytes.SequenceEqual(currentElementDown))
        {
            locationIndexes.Insert(0, foundIndexDown);         
            currentElementDown = _searchIndex.CitySearchIndex.Cities[foundIndexDown].Span.TrimEscapeBytesEnd();
            foundIndexDown--;
        }

        locationIndexes.Add(initialIndex);

        var foundIndexUp = initialIndex + 1;
        var currentElementUp = _searchIndex.CitySearchIndex.Cities[foundIndexUp].Span.TrimEscapeBytesEnd();

        while (foundIndexUp < _searchIndex.CitySearchIndex.Cities.Length - 1 && cityBytes.SequenceEqual(currentElementUp))
        {
            locationIndexes.Add(foundIndexUp);
            currentElementUp = _searchIndex.CitySearchIndex.Cities[foundIndexUp].Span.TrimEscapeBytesEnd();
            foundIndexUp++;
        }

        return locationIndexes;
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
