using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using LIE.DataTypes;

namespace LIE
{
    internal static class IOManager
    {
        public static List<T> ReadWithoutMapper<T>(string filePath) where T:class
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                return csv.GetRecords<T>().ToList();
            }

        }

        public static List<T> ReadWithMapper<T, TMapper>(string filePath) where TMapper:ClassMap
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<TMapper>();
                return csv.GetRecords<T>().ToList();
            }

        }
        public static void WriteWithoutMapper<T>(List<T> input, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer))
                csv.WriteRecords(input);
        }


        public static void WriteWithoutMapper<T>(IEnumerable<T> input, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer))
                csv.WriteRecords(input);
        }

        public static void WriteWithMapper<T, TMapper>(List<T> input, string filePath) where TMapper : ClassMap
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<TMapper>();
                csv.WriteRecords(input);
            }
        }
    }

    internal sealed class AlbumWithLocationMap:ClassMap<AlbumWithLocation>
    {
        public AlbumWithLocationMap()
        {
            Map(m => m.AlbumId).Name(":ID(Album)");
            Map(m => m.Name).Name("name");
            Map(m => m.Location).Name("location");
        }
    }

    public sealed class ArtistWithRolesMap:ClassMap<Artist>
    {
        public ArtistWithRolesMap()
        {
            Map(m => m.ArtistId).Name(":ID(Artist)");
            Map(m => m.Name).Name("name");
            Map(m => m.Type).Name(":LABEL").TypeConverter<ArtistTypeConvertor>();
        }
    }

    public class ArtistTypeConvertor : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            ArtistType result = ArtistType.None;
            Enum.TryParse(text, out result);
            return result;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return ((ArtistType) value).ToString();
        }
    }

    public sealed class TrackWithFileMap:ClassMap<TrackWithFile>
    {
        public TrackWithFileMap()
        {
            Map(m => m.TrackId).Name("trackID:ID(Track)");
            Map(m => m.Name).Name("name");
            Map(m => m.File).Name("file");
        }
    }
}
