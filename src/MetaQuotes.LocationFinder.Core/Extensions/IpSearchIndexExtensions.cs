using MetaQuotes.LocationFinder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaQuotes.LocationFinder.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IpSearchIndexExtensions
    {
        public static void Add(
            this IpSearchIndex searchIndex, 
            uint ipFrom, 
            uint ipTo, 
            uint locationIndex, 
            int index)
        {
            searchIndex.IpsFrom[index] = ipFrom;
            searchIndex.IpsTo[index] = ipTo;
            searchIndex.LocationIndexes[index] = locationIndex;
        }
    }
}
