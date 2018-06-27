using System;
using System.Configuration;
using System.Linq;
using CommandLine;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;
using Sciendo.T2F.Configuration;
using Sciendo.T2F.Processor;

namespace Sciendo.T2F
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            if (result.Tag==ParserResultType.Parsed)
            {
                var options = ((Parsed<Options>) result).Value;
                Console.WriteLine("Executing with argument: >>>{0}<<<",options.RootPath);
                var configSection = ((Tags2FilesConfigSection) ConfigurationManager.GetSection("tagsToFiles"));
                var extensions =
    configSection.Extensions
    .Cast<ExtensionElement>().Select(e => e.Value).ToArray();
                var individualRule = configSection.Rules
.Cast<RuleElement>().FirstOrDefault(r=>r.Type==RuleType.Individual);
                if(individualRule==null)
                    throw new ConfigurationErrorsException("no individual rule.");
                var collectionRule = configSection.Rules
.Cast<RuleElement>().FirstOrDefault(r => r.Type == RuleType.Collection);
                if(collectionRule==null)
                    throw new ConfigurationErrorsException("no collection rule.");

                IStorage storage = new FsStorage();
                storage.Directory.DirectoryRead += FileEnumerator_DirectoryRead;
                storage.Directory.ExtensionsRead += FileEnumerator_ExtensionsRead;

                T2FProcessor t2FProcessor = new T2FProcessor(storage,
                    new TagFileProcessor());
                t2FProcessor.Start(options.RootPath, extensions, individualRule.Pattern,
                    collectionRule.Pattern, configSection.ActionType.Type);
                return;
            }
            Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result));

        }

        private static void FileEnumerator_ExtensionsRead(object sender, ExtensionsReadEventArgs e)
        {
            Console.WriteLine("Processing extension: {0}",e.Extension);
        }

        private static void FileEnumerator_DirectoryRead(object sender, DirectoryReadEventArgs e)
        {
            Console.WriteLine("Processing directory: {0}",e.Directory);
        }
    }
}
