﻿using System;
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
                var extensions =
    ((ExtensionsWithTagsConfigSection)ConfigurationManager.GetSection("activeExtensions")).Extensions
    .Cast<ExtensionElement>().Select(e => e.Value).ToArray();

                IFileEnumerator fileEnumerator= new FileEnumerator();
                fileEnumerator.DirectoryRead += FileEnumerator_DirectoryRead;
                fileEnumerator.ExtensionsRead += FileEnumerator_ExtensionsRead;
                IDirectoryEnumerator directoryEnumerator= new DirectoryEnumerator();
                
                if (options.ActionType == ActionType.Copy)
                {
                    T2FProcessor t2FProcessor = new T2FProcessor(fileEnumerator,directoryEnumerator, new TagFileReader(),
                        new TagFileProcessor(), new ContentCopier(directoryEnumerator,fileEnumerator));
                    t2FProcessor.Start(options.RootPath, extensions, options.IndividualPattern,
                        options.CollectionPattern);
                    return;
                }
                if (options.ActionType == ActionType.Move)
                {
                    T2FProcessor t2FProcessor = new T2FProcessor(fileEnumerator,directoryEnumerator, new TagFileReader(),
                        new TagFileProcessor(), new ContentMover(directoryEnumerator, fileEnumerator));
                    t2FProcessor.Start(options.RootPath, extensions, options.IndividualPattern,
                        options.CollectionPattern);
                    return;
                }
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result));
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
