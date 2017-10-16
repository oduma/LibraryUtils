using System.IO;
using Sciendo.Common.IO;

namespace Sciendo.Playlist.Persister
{
    public class TextFileReader:IFileReader<string>
    {
        public string ReadFile(string filePath)
        {
            if(File.Exists(filePath))
                return File.ReadAllText(filePath);
            return "";
        }
    }
}
