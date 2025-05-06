using System.IO;
using System.Linq;
using TagLib;

namespace Sciendo.T2F.Processor
{
    public class TagFileProcessor:IFileProcessor<Tag>
    {
        public string CalculateFileName(Tag input, string rootPath, string extension, string fileNamePattern, bool isPartOfCollection)
        {
            var finalArtist = IOFy(
                (input.AlbumArtists.Any() && !isPartOfCollection) ? 
                string.Join("-",input.AlbumArtists):
                string.Join("-",input.Performers));

            return
                $"{rootPath}" +
                $"{Path.DirectorySeparatorChar}" +
                $"{string.Format(MapPattern(fileNamePattern), IOFy(input.Title), IOFy(input.Album), finalArtist, input.Track, input.Disc,CalculateCategory(finalArtist))}{extension}";
        }

        private string CalculateCategory(string inputArtist)
        {
            if (char.IsLetter(inputArtist[0]))
            {
                return inputArtist.Substring((inputArtist.ToLower().StartsWith("the ")) ? 4 : 0, 1).ToLower();
            }
            else
            {
                return "0-9";
            }
        }

        private string IOFy(string input)
        {

            foreach (char invalidInPath in Path.GetInvalidPathChars())
                input = input.Replace(invalidInPath, '_');
            input = input.Replace(':', '_').Replace('?', '_').Replace('*', '_').Replace('/','_').Replace('\\','_');
            return input;
        }

        private string MapPattern(string fileNamePattern)
        {
            return
                fileNamePattern.Replace("%a", "{2}")
                    .Replace("%l", "{1}")
                    .Replace("%t", "{0}")
                    .Replace("%n", "{3}")
                    .Replace("%d", "{4}")
                    .Replace("%z","{5}");
        }
    }
}
