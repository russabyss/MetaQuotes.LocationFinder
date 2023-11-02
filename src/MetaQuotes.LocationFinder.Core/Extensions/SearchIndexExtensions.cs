using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Helpers;
using MetaQuotes.LocationFinder.Core.Models;

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
    }
}
