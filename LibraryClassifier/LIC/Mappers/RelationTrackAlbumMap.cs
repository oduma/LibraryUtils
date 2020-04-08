using CsvHelper.Configuration;
using LIC.DataTypes;

namespace LIC.Mappers
{
    internal sealed class RelationTrackAlbumMap : ClassMap<RelationTrackAlbum>
    {
        public RelationTrackAlbumMap()
        {
            Map(m => m.TrackId).Name(":START_ID(Track)");
            Map(m => m.TrackNo).Name("track_no");
            Map(m => m.AlbumId).Name(":END_ID(Album)");
        }
    }
}