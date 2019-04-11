using CsvHelper.Configuration;
using LIE.DataTypes;

namespace LIE.Mappers
{

    public sealed class ArtistMap : ClassMap<Artist>
    {
        public ArtistMap()
        {
            Map(m => m.ArtistId).Name("artistID:ID(Artist)");
            Map(m => m.Name).Name("name");
            Map(m => m.ArtistLabels).Name(":LABEL");
        }
    }
}
