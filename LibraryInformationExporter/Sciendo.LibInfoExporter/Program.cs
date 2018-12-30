using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Common.IO;

namespace Sciendo.LibInfoExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var fsStorage = new FsStorage();
            ArtistNameExporter artistNameExporter= new ArtistNameExporter(fsStorage);

            using (var fs = File.CreateText("artistNames.csv"))
            {
                fs.WriteLine("Id,Artist,Role");
                int i = 1;
                foreach (var artistWithRole in artistNameExporter.GetFullListOfArtistNames(@"c:\users\octo\Music\",
                    new[] {".mp3", ".flac", ".wma", ".m4a"}))
                {
                    Console.WriteLine("{2}. Writing: {0} as a {1}.",artistWithRole.Name,artistWithRole.Role,i);
                    fs.WriteLine($"art{i++},{artistWithRole.Name},{artistWithRole.Role}");
                }
            }
        }
    }
}
