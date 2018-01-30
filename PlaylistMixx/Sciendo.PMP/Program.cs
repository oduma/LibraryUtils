using System;
using System.Collections.Generic;
using System.Configuration;
using CommandLine;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;
using Sciendo.Mixx.DataAccess;
using Sciendo.Mixx.DataAccess.Domain;
using Sciendo.Playlist.Mixx.Processor;
using Sciendo.Playlists;
using Sciendo.PMP.Configuration;

namespace Sciendo.PMP
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            if (result.Tag == ParserResultType.Parsed)
            {
                var options = ((Parsed<Options>) result).Value;
                MixxxConfigurationSection mixxxConfig = ConfigurationManager.GetSection("mixxx") as MixxxConfigurationSection;

                IMap<IEnumerable<PlaylistItem>,IEnumerable<MixxxPlaylistTrack> > trackMapper= new MapTracks();
                if (mixxxConfig != null)
                {
                    IDataHandler dataHandler= new DataHandler(trackMapper, $"Data Source={mixxxConfig.MixxxDatabaseFile};version=3;"); 
                    var processorFactory = new MixxxProcessorFactory();
                    var processor = processorFactory.GetProcessor(options.ProcessingType, dataHandler,
                        new TextFileReader(), new TagFileReader(), new TextFileWriter());
                    processor.MixxxPlaylistCreated += Processor_MixxxPlaylistCreated;
                    processor.MixxxPlaylistDeleted += Processor_MixxxPlaylistDeleted;
                    processor.Start(options.PlaylistFileName);
                }
            }
            else
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result));
            }
        }

        private static void Processor_MixxxPlaylistDeleted(object sender, MixxxProcessorProgressEventHandler e)
        {
            Console.WriteLine("Deleted mixxx Playlist {0}.",e.MixxxPlaylistName);
        }

        private static void Processor_MixxxPlaylistCreated(object sender, MixxxProcessorProgressEventHandler e)
        {
            Console.WriteLine("Created mixxx Playlist {0}.",e.MixxxPlaylistName);
        }
    }
}
