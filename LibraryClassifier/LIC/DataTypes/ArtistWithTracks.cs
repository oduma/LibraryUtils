using System;
using System.Collections.Generic;

namespace LIC.DataTypes
{
    public class ArtistWithTracks
    {
        public Guid ArtistId { get; set; }
        public string Name { get; set; }
        public List<TrackByArtist> Tracks { get; set; }
    }
}
