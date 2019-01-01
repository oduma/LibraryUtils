using CsvHelper.Configuration;
using LIE.DataTypes;

namespace LIE.Mappers
{
    internal sealed class RelationComposerTrackMap:ClassMap<RelationComposerTrack>
    {
        public RelationComposerTrackMap()
        {
            Map(m => m.ArtistId).Name(":START_ID(Artist)");
            Map(m => m.TrackId).Name(":END_ID(Track)");
        }
    }
}