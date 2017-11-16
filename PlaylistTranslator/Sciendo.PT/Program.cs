using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Sciendo.Common.IO;
using Sciendo.Playlist.Translator;
using Sciendo.Playlist.Translator.Configuration;

namespace Sciendo.PT
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<Options>(args);
            if (result.Tag== ParserResultType.Parsed)
            {
                var options = ((Parsed<Options>) result).Value;
                Console.WriteLine("Arguments Ok starting...");
                var extensions =
((ExtensionsPlaylistsConfigSection)ConfigurationManager.GetSection("activeExtensions")).Extensions
.Cast<ExtensionElement>().Select(e => e.Value).ToArray();
                var fromToParams =
                    GetSortedParams(((FindAndReplaceConfigSection) ConfigurationManager.GetSection("findReplaceSection"))
                        .FromToParams
                        .Cast<FromToParamsElement>().Select(e => e).OrderBy(e=>e.Priority));
                IFileEnumerator fileEnumerator = new FileEnumerator();
                ITranslator translator= new Translator(options.Source,extensions,options.Destination,fromToParams,fileEnumerator,new TextFileReader(), new TextFileWriter());
                translator.PathTranslated += Translator_PathTranslated;
                translator.Start();
                Console.WriteLine("Finished running.");
            }
            else
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result));

        }

        private static Dictionary<string, string> GetSortedParams(IEnumerable<FromToParamsElement> fromToParams)
        {
            var result = new Dictionary<string, string>();
            foreach (var fromToParam in fromToParams)
            {
                result.Add(fromToParam.From,fromToParam.To);
            }
            return result;
        }

        private static void Translator_PathTranslated(object sender, PathEventArgs e)
        {
            Console.WriteLine("Translated: {0}",e.Path);
        }
    }
}
