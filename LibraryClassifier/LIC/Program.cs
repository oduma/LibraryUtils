using LIC.Configuration;
using LIC.DataTypes;
using LIC.IoAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sciendo.ArtistClassifier.Contracts;
using Sciendo.ArtistClassifier.Logic;
using Sciendo.Config;
using Sciendo.MusicClassifier.KnowledgeBaseProvider;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LIC
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = ConfigureLog(serviceCollection);
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var classifierConfiguration = ReadConfiguration(logger, args);


            logger.LogInformation("Loading tags from file: {0}...", classifierConfiguration.NotProcessedFile);
            var allTags = IoManager.ReadWithoutMapper<FileWithTags>(classifierConfiguration.NotProcessedFile).ToList();
            logger.LogInformation("Loaded {0} tags.", allTags.Count);
            ProcessAllTags(ConfigureServices(serviceCollection, classifierConfiguration),allTags, classifierConfiguration);
            logger.LogInformation("Processing done!");
        }

        private static ServiceProvider ConfigureServices(ServiceCollection serviceCollection, ClassifierConfiguration classifierConfiguration)
        {

            serviceCollection.AddTransient<IKnowledgeBaseFactory, KnowledgeBaseFactory>();
            serviceCollection.AddTransient<IArtistProcessor>(r => new ArtistProcessor(r.GetService<IKnowledgeBaseFactory>(), GetKnoledgeBaseFilePath(classifierConfiguration.KnowledgeBase)));
            return serviceCollection.BuildServiceProvider();
        }

        private static string GetKnoledgeBaseFilePath(string knowledgeBase)
        {
            if (Regex.Match(knowledgeBase, @"^.?\:").Success)
                return knowledgeBase;
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, knowledgeBase);
        }

        private static void ProcessAllTags(ServiceProvider serviceProvider, List<FileWithTags> allTags, 
            ClassifierConfiguration classifierConfiguration)
        {
            //build a tree for artists
            List<ArtistWithTracks> allArtists = new List<ArtistWithTracks>();
            //build a tree for albums
            List<AlbumWithLocationAndTracks> allAlbums = new List<AlbumWithLocationAndTracks>();
            //build a list of tracks
            List<TrackWithFile> allTracks = new List<TrackWithFile>();

            GetAllTrees(serviceProvider.GetService<ILogger<Program>>(), serviceProvider.GetService<IArtistProcessor>(),
                classifierConfiguration, allTags, allArtists, allAlbums, allTracks);

            WriteNodes(serviceProvider.GetService<ILogger<Program>>(), classifierConfiguration, 
                allArtists, allAlbums, allTracks);

            WriteRelationships(serviceProvider.GetService<ILogger<Program>>(), classifierConfiguration, 
                allAlbums, allArtists);
        }

        private static void GetAllTrees(ILogger<Program> logger, IArtistProcessor artistProcessor, ClassifierConfiguration config, List<FileWithTags> allFilesWithTags,
    List<ArtistWithTracks> allArtists, List<AlbumWithLocationAndTracks> allAlbums, List<TrackWithFile> allTracks)
        {
            logger.LogInformation("Extracting information from Tags...");

            var done = 0;
            foreach (var tag in allFilesWithTags)
            {
                logger.LogInformation("Processing file: {1} of {2}", done++, allFilesWithTags.Count);
                var trackWithFile = tag.ExtractTrack();
                allArtists.Merge(tag.ExtractArtists(artistProcessor, trackWithFile));
                allAlbums.Merge(tag.ExtractAlbum(trackWithFile));
                allTracks.Add(trackWithFile);
                logger.LogInformation("Artists Found: {0}, Albums Found: {1}; Tracks Found: {2}",
                    allArtists.Count, allAlbums.Count,allTracks.Count) ;
            }
            logger.LogInformation("Artists Found: {0}, Albums Found: {1}; Tracks Found: {2}",
                allArtists.Count, allAlbums.Count, allTracks.Count);
        }

        private static void WriteNodes(ILogger<Program> logger, ClassifierConfiguration config, 
            List<ArtistWithTracks> allArtists, List<AlbumWithLocationAndTracks> allAlbums, List<TrackWithFile> allTracks)
        {
            logger.LogInformation("Writing artist names...");
            var allArtistNodes = allArtists.AggregateArtists().MapToNodes();
            allArtistNodes.WriteFile(config.Facts.AllArtistsFile);
            logger.LogInformation("Written {0} artist nodes.", allArtistNodes.Count);

            logger.LogInformation("Writing album names...");
            allAlbums.Select(a => (AlbumWithLocation)a).OrderBy(a => a.Name).ToList()
                .WriteFile(config.Facts.AllAlbumsFile);
            Console.WriteLine("Written {0} album nodes.", allAlbums.Count);

            Console.WriteLine("Writing track names...");
            allTracks.OrderBy(t => t.Name).ToList().WriteFile(config.Facts.AllTracksFile);
            Console.WriteLine("Written {0} track nodes.", allTracks.Count);
        }

        private static void WriteRelationships(ILogger<Program> logger, ClassifierConfiguration config, 
            List<AlbumWithLocationAndTracks> allAlbums, List<ArtistWithTracks> allArtists)
        {
            logger.LogInformation("Writing track to album relations...");
            var relationsTrackAlbum = allAlbums.GetRelationsTrackAlbum().ToList()
                .WriteFile(config.Relations.AlbumTrackFile);
            logger.LogInformation("Written {0} relations between tracks and albums.", relationsTrackAlbum.Count);

            logger.LogInformation("Writing composer to track relations...");
            var relationsComposerTrack = allArtists.SelectMany(RelationsHelper.GetRelationsComposerTrack).ToList()
                .WriteFile(config.Relations.ComposerTrackFile);
            logger.LogInformation("Written {0} relations between composers and tracks.",
                relationsComposerTrack.Count);

            logger.LogInformation("Writing artist to track relations...");
            var relationsArtistTrack = allArtists.SelectMany(RelationsHelper.GetRelationsArtistTrack).ToList()
                .WriteFile(config.Relations.ArtistTrackFile);
            logger.LogInformation("Extracted {0} relations between artists and tracks.",
                relationsArtistTrack.Count);

            logger.LogInformation("Writing featured artist relations to Tracks...");

            var relationsFeaturedArtistTrack = allArtists.SelectMany(RelationsHelper.GetRelationsFeaturedArtistTrack).ToList()
                .WriteFile(config.Relations.FeaturedArtistTrackFile);
            logger.LogInformation("Written {0} relations to tracks from existing featured artists.",
                relationsFeaturedArtistTrack.Count);
        }

        private static ClassifierConfiguration ReadConfiguration(ILogger<Program> logger, string[] args)
        {
            try
            {
                return new ConfigurationManager<ClassifierConfiguration>().GetConfiguration(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"{AppDomain.CurrentDomain.FriendlyName}.json")
                    .AddCommandLine(args)
                    .Build());
            }
            catch (Exception e)
            {
                logger.LogError(e, "wrong config!");
                throw e;
            }
        }

        private static ServiceProvider ConfigureLog(ServiceCollection services)
        {
            return services.AddLogging(configure => configure.AddSerilog(new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger())).BuildServiceProvider();
        }
    }
}
