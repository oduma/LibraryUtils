using CsvHelper.Configuration;
using LIE.DataTypes;

namespace LIE.Mappers
{

    internal sealed class AlbumWithLocationMap : ClassMap<AlbumWithLocation>
    {
        public AlbumWithLocationMap()
        {
            Map(m => m.AlbumId).Name(":ID(Album)");
            Map(m => m.Name).Name("name");
            Map(m => m.Location).Name("location");
        }
    }
}
