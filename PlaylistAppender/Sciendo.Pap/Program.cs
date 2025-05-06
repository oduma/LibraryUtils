using CommandLine;
using Sciendo.Common.IO;
using Sciendo.Playlist.Appender;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Pap
{
    class Program
    {
        static void Main(string[] args)
        {
            var drives = DriveInfo.GetDrives();
            foreach(var drive in drives)
            {
                Console.WriteLine("Drive: {0}", drive.Name);
            }

            //var result = Parser.Default.ParseArguments<Options>(args);
            //if (result.Tag == ParserResultType.Parsed)
            //{
            //    var options = ((Parsed<Options>)result).Value;
            //    Console.WriteLine("Arguments Ok starting...");
            //    Console.WriteLine("Finished running.");
            //    IStorage inStorage= StorageProviderFactory.GetStorageProvider(options.InPlaylistsPath);
            //    IStorage outStorage = StorageProviderFactory.GetStorageProvider(options.OutPlaylistsPath);
            //    IPlaylistReader sourcePlaylistReader = new PlaylistReader(options.InPlaylistsPath, inStorage);
            //    IPlaylistReader targetPlaylistReader = new PlaylistReader(options.OutPlaylistsPath, outStorage);
            //    IPlaylistAppender playlistAppender = new PlaylistAppender(
            //        sourcePlaylistReader,
            //        targetPlaylistReader,
            //        new PlaylistTransformer(),
            //        new PlaylistCombiner(),
            //        new PlaylistWriter(outStorage, options.OutPlaylistsPath));
            //    playlistAppender.Append(
            //        options.InPlaylistsPath, 
            //        options.SourcePlaylistMusicRootPath, 
            //        options.OutPlaylistsPath, 
            //        options.TargetPlaylistMusicRootPath);
            //    return;
            //}
            //Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result));
        }
    }
}
