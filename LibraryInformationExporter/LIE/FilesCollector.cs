using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using LIE.DataTypes;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;

namespace LIE
{
    internal class FilesCollector
    {
        private readonly FsStorage _fsStorage;
        public event EventHandler<ProgressEventArgs> Progress;
        private static bool MessageStop = false;
        public FilesCollector(FsStorage fsStorage)
        {
            _fsStorage = fsStorage;
        }
        
        public IEnumerable<FileWithTags> ScanPath(string folderPath, string[] extensions)
        {
            Progress?.Invoke(this, new ProgressEventArgs($"Starting scanning from folder {folderPath}..."));
            Progress?.Invoke(this,
                new ProgressEventArgs(
                    $"Detected {_fsStorage.Directory.GetFiles(folderPath, SearchOption.AllDirectories, extensions).Count()} files to be scanned ..."));

            return _fsStorage.Directory.GetFiles(folderPath, SearchOption.AllDirectories, extensions).AsParallel()
                .Select(GetFileWithTagsForFile);
        }

        private FileWithTags GetFileWithTagsForFile(string filePath)
        {
            if (DateTime.Now.Second % 5 == 0 && !MessageStop)
            {
                Progress?.Invoke(this, new ProgressEventArgs($"Processing file: {filePath}"));
                MessageStop = true;
            }

            if (DateTime.Now.Second % 8 == 0)
            {
                MessageStop = false;
            }
                TagLib.Tag tag;
                try
                {
                    tag = _fsStorage.File.ReadTag(filePath).Tag;
                }
                catch
                {
                    tag = null;
                }

                if (tag != null && !tag.IsEmpty)
                {
                    return new FileWithTags
                    {
                        AlbumArtists = tag.JoinedAlbumArtists,
                        Artists = tag.JoinedPerformers,
                        Composers = tag.JoinedComposers,
                        Album = tag.Album,
                        Title = tag.Title,
                        Year = tag.Year.ToString(),
                        Track = tag.Track.ToString(),
                        FilePath = filePath,
                        TrackId = Guid.NewGuid()
                    };
                }

            return null;
        }

        public List<FileWithTags> GetFullListOfTagsFromFile(string filePath)
        {
            Progress?.Invoke(this, new ProgressEventArgs($"Loading all tags for files from file {filePath}..."));
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                return csv.GetRecords<FileWithTags>().ToList();
            }
        }

        public static List<ArtistWithRoles> LoadAllArtistNamesFromFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<ArtistWithRolesMap>();
                return csv.GetRecords<ArtistWithRoles>().ToList();
            }
        }
        public static List<AlbumWithLocation> LoadAllAlbumNamesFromFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<AlbumWithLocationMap>();
                return csv.GetRecords<AlbumWithLocation>().ToList();
            }
        }

    }

    internal sealed class AlbumWithLocationMap:ClassMap<AlbumWithLocation>
    {
        public AlbumWithLocationMap()
        {
            Map(m => m.AlbumId).Name(":ID(Album)");
            Map(m => m.Name).Name("album");
            Map(m => m.Location).Name("location");
        }
    }

    public sealed class ArtistWithRolesMap:ClassMap<ArtistWithRoles>
    {
        public ArtistWithRolesMap()
        {
            Map(m => m.ArtistId).Name(":ID(Artist)");
            Map(m => m.Name).Name("artist");
            Map(m => m.ArtistRoles).Name(":LABEL").TypeConverter<ArtistRolesConvertor>();
        }
    }

    public class ArtistRolesConvertor : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            List<ArtistRole> result = new List<ArtistRole>();
            if (!string.IsNullOrEmpty(text))
            {
                var textParts = text.Split(';');
                foreach (var textPart in textParts)
                {
                    if (Enum.TryParse(textPart, true, out ArtistRole currentArtistRole))
                    {
                        result.Add(currentArtistRole);
                    }
                }
            }

            return result;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            List<ArtistRole> input = (List<ArtistRole>) value;
            return string.Join(";", input);
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
