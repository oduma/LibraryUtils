using System.Collections.Generic;

namespace LIE
{
    internal class AlbumWithLocationEqualityComparer : IEqualityComparer<AlbumWithLocation>
    {
        public bool Equals(AlbumWithLocation x, AlbumWithLocation y)
        {
            if (string.IsNullOrEmpty(x.Location) || string.IsNullOrEmpty(y.Location))
                return false;
            if (x.Location == y.Location)
                return true;
            return false;
        }

        public int GetHashCode(AlbumWithLocation obj)
        {
            return obj.Location.GetHashCode();
        }
    }
}