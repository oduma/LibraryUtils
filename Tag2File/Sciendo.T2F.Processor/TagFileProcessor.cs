using System.IO;
using TagLib;

namespace Sciendo.T2F.Processor
{
    public class TagFileProcessor:IFileProcessor<Tag>
    {
        public string CalculateFileName(Tag input, string rootPath, string extension, string fileNamePattern)
        {
            return
                $"{rootPath}{Path.DirectorySeparatorChar}{string.Format(MapPattern(fileNamePattern), input.Title, input.Album, string.Join("-", input.AlbumArtists), string.Join("-", input.Artists), input.Track, input.Disc)}{extension}";
        }

        private string MapPattern(string fileNamePattern)
        {
            return
                fileNamePattern.Replace("%aa", "{2}")
                    .Replace("%a", "{3}")
                    .Replace("%l", "{1}")
                    .Replace("%t", "{0}")
                    .Replace("%n", "{4}")
                    .Replace("%d", "{5}");
        }
    }
}
