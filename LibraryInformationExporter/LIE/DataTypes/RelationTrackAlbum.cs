using System;

namespace LIE.DataTypes
{
    internal class RelationTrackAlbum
    {
        public Guid TrackId { get; set; }
        public string TrackNo { get; set; }
        public Guid AlbumId { get; set; }
    }
}