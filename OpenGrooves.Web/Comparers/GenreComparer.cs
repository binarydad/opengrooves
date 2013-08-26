using OpenGrooves.Web.Models;
using System.Collections.Generic;

namespace OpenGrooves.Web.Comparers
{
    public class GenreComparer : IEqualityComparer<GenreModel>
    {
        public bool Equals(GenreModel x, GenreModel y)
        {
            return x.GenreId == y.GenreId;
        }

        public int GetHashCode(GenreModel obj)
        {
            return obj.GenreId.GetHashCode();
        }
    }
}