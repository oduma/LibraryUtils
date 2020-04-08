using System.Collections.Generic;

namespace LIC.DataTypes
{
    public class AlbumWithLocationAndTracks : AlbumWithLocation
    {
        public List<TrackWithFile> Tracks { get; set; }
    }
}
