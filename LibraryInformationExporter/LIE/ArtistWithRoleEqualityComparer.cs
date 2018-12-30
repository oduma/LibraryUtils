using System.Collections.Generic;

namespace LIE
{
    internal class ArtistWithRoleEqualityComparer : IEqualityComparer<ArtistWithRole>
    {
        public bool Equals(ArtistWithRole x, ArtistWithRole y)
        {
            if (x == null)
                return false;
            if (y == null)
                return false;
            if (x.Name.ToLower() == y.Name.ToLower() && x.Role==y.Role)
                return true;
            return false;
        }

        public int GetHashCode(ArtistWithRole obj)
        {
            return (obj.Name.ToLower() + obj.Role).GetHashCode();
        }
    }
}