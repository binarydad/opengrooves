using OpenGrooves.Web.Models;
using System.Collections.Generic;

namespace OpenGrooves.Web.Comparers
{
    public class BandComparer : IEqualityComparer<BandModel>
    {
        public bool Equals(BandModel x, BandModel y)
        {
            return x.BandId == y.BandId;
        }

        public int GetHashCode(BandModel obj)
        {
            return obj.BandId.GetHashCode();
        }
    }
}