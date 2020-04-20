using AE.DataTypes;
using CsvHelper.Configuration;

namespace AE.Mappers
{
    internal sealed class RelationBandArtistMap : ClassMap<RelationBandArtist>
    {
        public RelationBandArtistMap()
        {
            Map(m => m.BandId).Name(":START_ID(Artist)");
            Map(m => m.ArtistId).Name(":END_ID(Artist)");
        }
    }
}