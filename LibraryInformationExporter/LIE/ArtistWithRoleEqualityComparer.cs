using System.Collections.Generic;
using LIE.DataTypes;

namespace LIE
{
    internal class ArtistWithRoleEqualityComparer : IEqualityComparer<Artist>
    {
        public bool Equals(Artist x, Artist y)
        {
            if (x == null)
                return false;
            if (y == null)
                return false;
            if (x.Name.ToLower() == y.Name.ToLower() && x.Type==y.Type)
                return true;
            return false;
        }

        public int GetHashCode(Artist obj)
        {
            return (obj.Name.ToLower() + obj.Type).GetHashCode();
        }
    }
}