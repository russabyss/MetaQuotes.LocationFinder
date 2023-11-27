using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Helpers;
using MetaQuotes.LocationFinder.Core.Models;

namespace MetaQuotes.LocationFinder.Core.Extensions
{
    /// <summary>
    /// Методы расширения для <see cref="SearchIndex"/>.
    /// </summary>
    public static class SearchIndexExtensions
    {
        /// <summary>
        /// Получить локацию (<see cref="Location"/>) по номеру локации в массиве локаций.
        /// </summary>
        /// <param name="searchIndex">Поисковый индекс.</param>
        /// <param name="arrayIndex">Номер локации в массиве локаций.</param>
        /// <returns>Локация. См.<see cref="Location"/>.</returns>
        public static Location GetLocationByArrayIndex(this SearchIndex searchIndex, int arrayIndex)
        {
            var locationIndex = (int)searchIndex.IpSearchIndex.LocationIndexes[arrayIndex];
            var locationSpan = searchIndex.LocationsBytes.Slice(
                locationIndex * DbConstants.LocationLength, 
                DbConstants.LocationLength);

            return DbReaderHelper.GetLocation(locationSpan);
        }

        /// <summary>
        /// Получить локацию (<see cref="Location"/>) по номеру адреса локации в исходном файле в массиве адресов локаций.
        /// </summary>
        /// <param name="searchIndex">Поисковый индекс.</param>
        /// <param name="addressIndex">Номер адреса в списке адресов локаций.</param>
        /// <returns></returns>
        public static Location GetLocationByAddressInFile(this SearchIndex searchIndex, int addressIndex)
        {
            var addressOfLocation = searchIndex.CitySearchIndex.LocationAddresses[addressIndex];
            var locationSlice = searchIndex.LocationsBytes.Slice(
                addressOfLocation, 
                DbConstants.LocationLength);

            return  DbReaderHelper.GetLocation(locationSlice);
        }
    }
}
