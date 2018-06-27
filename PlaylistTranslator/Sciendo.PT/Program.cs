using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CommandLine;
using Sciendo.Common.IO;
using Sciendo.Common.IO.MTP;
using Sciendo.Playlist.Translator;
using Sciendo.Playlist.Translator.Configuration;

namespace Sciendo.PT
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            if (result.Tag== ParserResultType.Parsed)
            {
                var options = ((Parsed<Options>) result).Value;
                Console.WriteLine("Arguments Ok starting...");
                IStorage sourceStorage = CreateStorage(options.Source);
                IStorage targetStorage = CreateStorage(options.Destination);
                var extensions =
((ExtensionsPlaylistsConfigSection)ConfigurationManager.GetSection("activeExtensions")).Extensions
.Cast<ExtensionElement>().Select(e => e.Value).ToArray();
                var fromToParams =
                    GetSortedParams(((FindAndReplaceConfigSection) ConfigurationManager.GetSection("findReplaceSection"))
                        .FromToParams
                        .Cast<FromToParamsElement>().Select(e => e).OrderBy(e=>e.Priority));
                IBulkTranslator translator = new BulkTranslator(options.Source, extensions, options.Destination,
                    sourceStorage, targetStorage);
                translator.PathTranslated += Translator_PathTranslated;
                translator.Start(fromToParams);
                Console.WriteLine("Finished running.");
            }
            else
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result));

        }

        private static IStorage CreateStorage(string path)
        {
            if (MtpPathInterpreter.IsMtpDevice(path))
            {
                return new MtpStorage();
            }
            return new FsStorage();
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
