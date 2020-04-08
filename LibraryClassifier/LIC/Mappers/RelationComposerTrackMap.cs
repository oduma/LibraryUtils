using CsvHelper.Configuration;
using LIC.DataTypes;

namespace LIC.Mappers
{
    internal sealed class RelationComposerTrackMap : ClassMap<RelationComposerTrack>
    {
        public RelationComposerTrackMap()
        {
            Map(m => m.ArtistId).Name(":START_ID(Artist)");
            Map(m => m.TrackId).Name(":END_ID(Track)");
        }
    }
}