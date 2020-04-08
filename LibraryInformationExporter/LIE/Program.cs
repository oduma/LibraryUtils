using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using CommandLine;
using LIE.Configuration;
using LIE.DataTypes;
using Newtonsoft.Json;
using Sciendo.Common.IO;

namespace LIE
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("reading configuration...");
            var config = ConfigurationManager.GetSection("libraryImporter") as LibraryImporterConfigurationSection;
            List<FileWithTags> allTags = GetAllTags(config);
            Console.WriteLine("Finished.");
            Console.ReadKey();
        }

        private static List<string> _errorsInTags;
        private static List<FileWithTags> GetAllTags(LibraryImporterConfigurationSection config)
        {
            List<FileWithTags> allTags;
            Console.WriteLine("Initializing the TagProvider...");
            _errorsInTags= new List<string>();
            var tagsProvider = new TagsProvider(new FsStorage());
            tagsProvider.Progress += TagsProvider_Progress;
            tagsProvider.ProgressWithError += TagsProvider_ProgressWithError;
            allTags = tagsProvider.ScanPath(config.LibraryRootFolder,
                    config.Extensions
                        .Cast<ExtensionElement>().Select(e => e.Value).ToArray())
                .WriteFile(config.DataFiles.NotProcessed.AllTagsFile);
            _errorsInTags.WriteFile(config.DataFiles.NotProcessed.ErrorsFile);
            Console.WriteLine("{0} Tags collected from scan. Found {1} files with errors.", allTags.Count,_errorsInTags.Count);

            return allTags;
        }

        private static void TagsProvider_Progress(object sender, TagProviderProgressEventArgs e)
        {
            Console.WriteLine("{0} {1}",DateTime.Now, e.Message);
        }

        private static void TagsProvider_ProgressWithError(object sender, TagProviderProgressEventArgs e)
        {   
            _errorsInTags.Add(e.Message);
        }
    }
}
