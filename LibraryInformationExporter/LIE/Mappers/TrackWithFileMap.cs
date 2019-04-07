using CsvHelper.Configuration;
using LIE.DataTypes;

namespace LIE.Mappers
{


    public sealed class TrackWithFileMap : ClassMap<TrackWithFile>
    {
        public TrackWithFileMap()
        {
            Map(m => m.TrackId).Name("trackID:ID(Track)");
            Map(m => m.Name).Name("name");
            Map(m => m.File).Name("file");
        }
    }
}
