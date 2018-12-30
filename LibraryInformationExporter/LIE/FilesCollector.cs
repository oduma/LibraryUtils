using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;
using TagLib;
using File = System.IO.File;

namespace LIE
{
    internal class FilesCollector
    {
        public event EventHandler<TagPartReadEventArgs> FolderRead;

        
        public List<FileWithTag> ScanFolder(FsStorage fsStorage, string folderPath, string[] extensions)
        {
            List<FileWithTag> filesWithTags= new List<FileWithTag>();
            string folder = string.Empty;
            foreach (var filePath in fsStorage.Directory.GetFiles(folderPath, SearchOption.AllDirectories, extensions))
            {
                string newFolder = Path.GetDirectoryName(filePath);
                if (folder != newFolder)
                {
                    FolderRead?.Invoke(this, new TagPartReadEventArgs(newFolder));
                    folder = newFolder;
                }
                TagLib.Tag tag;
                try
                {
                    tag = fsStorage.File.ReadTag(filePath).Tag;
                }
                catch
                {
                    tag = null;
                }

                if (tag != null && !tag.IsEmpty)
                {
                    filesWithTags.AddRange(GetAllAvailableTagParts(tag,filePath));
                }
            }

            return filesWithTags;
        }

        private List<FileWithTag> GetAllAvailableTagParts(Tag tag, string filePath)
        {
            List<FileWithTag> fileWithTags= new List<FileWithTag>();
            if (!string.IsNullOrEmpty(tag.JoinedAlbumArtists))
                fileWithTags.Add(new FileWithTag
                    { FilePath = filePath, TagType = TagType.AlbumArtist, TagContents = tag.JoinedAlbumArtists });

            if (!string.IsNullOrEmpty(tag.JoinedPerformers))
                fileWithTags.Add(new FileWithTag
                    { FilePath = filePath, TagType = TagType.Artist, TagContents = tag.JoinedPerformers });

            if (!string.IsNullOrEmpty(tag.JoinedComposers))
                fileWithTags.Add(new FileWithTag
                    { FilePath = filePath, TagType = TagType.Composers, TagContents = tag.JoinedComposers });
            if (!string.IsNullOrEmpty(tag.Album))
                fileWithTags.Add(new FileWithTag
                    { FilePath = filePath, TagType = TagType.Album, TagContents = tag.Album });
            if (!string.IsNullOrEmpty(tag.Title))
                fileWithTags.Add(new FileWithTag
                    { FilePath = filePath, TagType = TagType.Title, TagContents = tag.Title });
            if (tag.Year > 0)
                fileWithTags.Add(new FileWithTag
                    { FilePath = filePath, TagType = TagType.Year, TagContents = tag.Year.ToString() });
            if (tag.Track > 0)
                fileWithTags.Add(new FileWithTag
                    { FilePath = filePath, TagType = TagType.Track, TagContents = tag.Track.ToString() });

            return fileWithTags;
        }

        public static List<FileWithTag> GetFullListOfTagsFromFile(string filePath, string titleLine)
        {
            List<FileWithTag> filesWithTags = new List<FileWithTag>();
            foreach (var line in File.ReadAllLines(filePath))
            {
                if (line != titleLine && !string.IsNullOrEmpty(line))
                {
                    var lineParts = line.Split(new[] { '\t' });
                    if (Enum.TryParse(lineParts[1], true, out TagType currenTagType))
                    {
                        filesWithTags.Add(new FileWithTag
                            {FilePath = lineParts[0], TagType = currenTagType, TagContents = lineParts[2]});
                    }
                }
            }

            return filesWithTags;
        }

        public static List<TrackWithFile> LoadAllTrackNamesFromFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<TrackWithFileMap>();
                return csv.GetRecords<TrackWithFile>().ToList();
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

    internal class AlbumWithLocationMap:ClassMap<AlbumWithLocation>
    {
        public AlbumWithLocationMap()
        {
            Map(m => m.AlbumId).Index(0);
            Map(m => m.Name).Index(1);
            Map(m => m.Location).Index(2);
        }
    }

    public class ArtistWithRolesMap:ClassMap<ArtistWithRoles>
    {
        public ArtistWithRolesMap()
        {
            Map(m => m.ArtistId).Index(0);
            Map(m => m.Name).Index(1);
            Map(m => m.ArtistRoles).Index(2).TypeConverter<ArtistRolesConvertor<List<ArtistRole>>>();
        }
    }

    public class ArtistRolesConvertor<T> : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            List<ArtistRole> result = new List<ArtistRole>();
            if (!string.IsNullOrEmpty(text))
            {
                var textParts = text.Split(new[] {';'});
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
    }

    public class TrackWithFileMap:ClassMap<TrackWithFile>
    {
        public TrackWithFileMap()
        {
            Map(m => m.TrackId).Index(0);
            Map(m => m.Name).Index(1);
            Map(m => m.File).Index(2);
        }
    }
}
