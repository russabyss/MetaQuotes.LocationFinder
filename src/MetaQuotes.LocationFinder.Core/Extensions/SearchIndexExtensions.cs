using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Helpers;
using MetaQuotes.LocationFinder.Core.Models;
using System;

namespace MetaQuotes.LocationFinder.Core.Extensions
{
    public static class SearchIndexExtensions
    {
        public static Location GetLocationByArrayIndex(this SearchIndex searchIndex, int arrayIndex)
        {
            var locationIndex = (int)searchIndex.IpSearchIndex.LocationIndexes[arrayIndex];
            var locationSpan = searchIndex.LocationsBytes.Slice(
                locationIndex * DbConstants.LocationLength, 
                DbConstants.LocationLength);

            return DbReaderHelper.GetLocation(locationSpan);
        }

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
