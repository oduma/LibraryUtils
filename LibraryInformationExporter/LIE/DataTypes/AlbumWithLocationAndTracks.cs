using System.Collections.Generic;

namespace LIE.DataTypes
{
    public class AlbumWithLocationAndTracks:AlbumWithLocation
    {
        public List<TrackWithFile> Tracks { get; set; }
    }
}
