using System.Collections.Generic;
using LIE.DataTypes;

namespace LIE
{
    internal class RelationArtistTrackComparer : IEqualityComparer<RelationArtistTrack>
    {
        public bool Equals(RelationArtistTrack x, RelationArtistTrack y)
        {
            if (x == null)
                return false;
            if (y == null)
                return false;
            return (x.ArtistId == y.ArtistId && x.TrackId == y.TrackId);
        }

        public int GetHashCode(RelationArtistTrack obj)
        {
            return (obj.TrackId.ToString() + obj.ArtistId.ToString()).GetHashCode();
        }
    }
}