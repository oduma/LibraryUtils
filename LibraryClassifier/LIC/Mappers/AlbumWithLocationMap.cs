using CsvHelper.Configuration;
using LIC.DataTypes;

namespace LIC.Mappers
{

    internal sealed class AlbumWithLocationMap : ClassMap<AlbumWithLocation>
    {
        public AlbumWithLocationMap()
        {
            Map(m => m.AlbumId).Name("albumID:ID(Album)");
            Map(m => m.Name).Name("name");
            Map(m => m.Location).Name("location");
        }
    }
}
