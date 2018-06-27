using System;
using CommandLine;
using Sciendo.Common.IO;
using Sciendo.Common.IO.MTP;
using Sciendo.Common.Music.Tagging;
using Sciendo.Playlist.Persister;
using Sciendo.Playlists;

namespace Sciendo.PP
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            if (result.Tag==ParserResultType.Parsed)
            {
                var options = ((Parsed<Options>) result).Value;
                Console.WriteLine("Arguments Ok starting...");
                IStorage sourceStorage = CreateStorage(options.InPlaylistsPath);
                IStorage targetStorage = CreateStorage(options.OutPlaylistsPath);
                PersisterProcessor persisterProcessor = new PersisterProcessor(sourceStorage, targetStorage, options.TargetPlaylistType);
                persisterProcessor.StartProcessing += PersisterProcessor_StartProcessing;
                persisterProcessor.StartProcessingFile += PersisterProcessor_StartProcessingFile;
                persisterProcessor.CopyContentToTarget += PersisterProcessor_CopyContentToTarget;
                persisterProcessor.PlaylistCreated += PersisterProcessor_PlaylistCreated;
                persisterProcessor.Start(options.InPlaylistsPath, options.OutPlaylistsPath);
                Console.WriteLine("Finished running.");
                return;
            }
            Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result));
        }

        private static IStorage CreateStorage(string optionsPlaylistsPath)
        {
            if(MtpPathInterpreter.IsMtpDevice(optionsPlaylistsPath))
                return new MtpStorage();
            return new FsStorage();
        }

        private static void PersisterProcessor_PlaylistCreated(object sender, ProgressEventArgs e)
        {
            Console.WriteLine("Created playlist: {0}.",e.Path);
        }

        private static void PersisterProcessor_CopyContentToTarget(object sender, ProgressEventArgs e)
        {
            Console.WriteLine("Copied to target directory: {0} number of files {1}", e.Path, e.ExpectedItemsToProcess);
        }

        private static void PersisterProcessor_StartProcessingFile(object sender, ProgressEventArgs e)
        {
            Console.WriteLine("Starting processing playlist: {0}... Number of items expected to be processed: {1}",e.Path, e.ExpectedItemsToProcess);
        }

        private static void PersisterProcessor_StartProcessing(object sender, ProgressEventArgs e)
        {
            Console.WriteLine("Starting processing: {0}... Number of items expected to be processed: {1}",e.Path, e.ExpectedItemsToProcess);
        }
    }
}
