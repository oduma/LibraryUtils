using CsvHelper.Configuration;
using LIC.DataTypes;

namespace LIC.Mappers
{

    public sealed class ArtistNodeMap : ClassMap<ArtistNode>
    {
        public ArtistNodeMap()
        {
            Map(m => m.ArtistId).Name("artistID:ID(Artist)");
            Map(m => m.Name).Name("name");
            Map(m => m.ArtistLabel).Name(":LABEL");
        }
    }
}
