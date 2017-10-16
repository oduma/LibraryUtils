using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Common.IO;
using Sciendo.Playlist.Persister;

namespace Sciendo.PP
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            var result = CommandLine.Parser.Default.ParseArguments(args, options);
            if (result)
            {
                IFileEnumerator fileEnumerator = new FileEnumerator();
                PersisterProcessor persisterProcessor = new PersisterProcessor(fileEnumerator, new TextFileReader(),
                    new PlaylistAnaliserFactory(), options.MusicSourceRoot, options.MusicCurrentRoot,
                    new ContentCopier(new DirectoryEnumerator(), fileEnumerator));
                persisterProcessor.Start(options.PlaylistsDirectory);
            }
            Console.WriteLine(options.GetHelpText());
        }
    }
}
