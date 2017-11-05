using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Common.IO;
using Sciendo.Playlist.Translator;
using Sciendo.Playlist.Translator.Configuration;

namespace Sciendo.PT
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            var result = CommandLine.Parser.Default.ParseArguments(args, options);
            if (result)
            {
                Console.WriteLine("Arguments Ok starting...");
                var extensions =
((ExtensionsPlaylistsConfigSection)ConfigurationManager.GetSection("activeExtensions")).Extensions
.Cast<ExtensionElement>().Select(e => e.Value).ToArray();

                IFileEnumerator fileEnumerator = new FileEnumerator();
                ITranslator translator= new Translator(options.Source,extensions,options.Destination,options.Find,options.ReplaceWith,fileEnumerator,new TextFileReader(), new TextFileWriter());
                translator.PathTranslated += Translator_PathTranslated;
                translator.Start();
                Console.WriteLine("Finished running.");
            }
            Console.WriteLine(options.GetHelpText());

        }

        private static void Translator_PathTranslated(object sender, PathEventArgs e)
        {
            Console.WriteLine("Translated: {0}",e.Path);
        }
    }
}
