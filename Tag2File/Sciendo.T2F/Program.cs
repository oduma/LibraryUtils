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
                var actionType = configSection.ActionType.Type;


                IFileEnumerator fileEnumerator = new FileEnumerator();
                fileEnumerator.DirectoryRead += FileEnumerator_DirectoryRead;
                fileEnumerator.ExtensionsRead += FileEnumerator_ExtensionsRead;
                IDirectoryEnumerator directoryEnumerator= new DirectoryEnumerator();
                
                if (actionType == ActionType.Copy)
                {
                    T2FProcessor t2FProcessor = new T2FProcessor(fileEnumerator,directoryEnumerator, new TagFileReader(),
                        new TagFileProcessor(), new ContentCopier(directoryEnumerator,fileEnumerator));
                    t2FProcessor.Start(options.RootPath, extensions, individualRule.Pattern,
                        collectionRule.Pattern);
                    return;
                }
                if (actionType == ActionType.Move)
                {
                    T2FProcessor t2FProcessor = new T2FProcessor(fileEnumerator,directoryEnumerator, new TagFileReader(),
                        new TagFileProcessor(), new ContentMover(directoryEnumerator, fileEnumerator));
                    t2FProcessor.Start(options.RootPath, extensions, individualRule.Pattern,
                        collectionRule.Pattern);
                    return;
                }
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
