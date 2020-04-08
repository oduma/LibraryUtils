using System;

namespace LIC.DataTypes
{
    public class RelationTrackAlbum
    {
        public Guid TrackId { get; set; }
        public string TrackNo { get; set; }
        public Guid AlbumId { get; set; }
    }
}