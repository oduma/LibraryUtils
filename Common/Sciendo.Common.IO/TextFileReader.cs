using System.IO;

namespace Sciendo.Common.IO
{
    public class TextFileReader:IFileReader<string>
    {
        public string Read(string filePath)
        {
            if(File.Exists(filePath))
                return File.ReadAllText(filePath);
            return "";
        }
    }
}
