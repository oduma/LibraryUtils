using System;

namespace LIE.DataTypes
{
    public class RelationArtistTrack
    {
        public Guid ArtistId { get; set; }
        public string Year { get; set; }
        public Guid TrackId { get; set; }
    }
}