using System.Collections.Generic;

namespace LIE.DataTypes
{
    public class TrackByArtist:TrackWithFile
    {
        public ArtistType Type { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsComposer { get; set; }
    }
}