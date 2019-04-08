using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using CommandLine;
using LIE.Configuration;
using LIE.DataTypes;
using LIE.KnowledgeBaseTypes;
using Newtonsoft.Json;
using Sciendo.Common.IO;

namespace LIE
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);

            //var config = ConfigurationManager.GetSection("libraryImporter") as LibraryImporterConfigurationSection;
            //var knowledgeBase = JsonConvert.SerializeObject(new KnowledgeBase());
            //File.WriteAllText(config.KnowledgeBase, knowledgeBase);

            if (result.Tag == ParserResultType.Parsed)
            {
                var options = ((Parsed<Options>)result).Value;
                Console.WriteLine("Arguments Ok starting...");
                Console.WriteLine("reading configuration...");
                var config = ConfigurationManager.GetSection("libraryImporter") as LibraryImporterConfigurationSection;
                List<FileWithTags> allTags = GetAllTags(options.Rescan, config);
                if (options.Process)
                    ProcessTags(allTags, config);
                Console.WriteLine("Finished.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result));
        }

        private static readonly ProgressAccumulator Accumulator= new ProgressAccumulator();
        private static void ProcessTags(List<FileWithTags> allFilesWithTags, 
            LibraryImporterConfigurationSection config)
        {
            KnowledgeBase knowledgeBase = LoadKnowledgeBase(config.KnowledgeBase);
            ArtistNameExporter artistNameExporter = new ArtistNameExporter(knowledgeBase);
            //build a tree for artists
            List<ArtistWithTracks> allArtists = new List<ArtistWithTracks>();
            //build a tree for albums
            List<AlbumWithLocationAndTracks> allAlbums = new List<AlbumWithLocationAndTracks>();
            //build a list of tracks
            List<TrackWithFile> allTracks = new List<TrackWithFile>();

            GetAllTrees(artistNameExporter, allFilesWithTags, config, allArtists, allAlbums, allTracks);

            WriteNodes(config, artistNameExporter, allArtists, allAlbums, allTracks);

            WriteRelationships(config, allAlbums, allArtists);
        }

        private static KnowledgeBase LoadKnowledgeBase(string knowledgeBaseFile)
        {
            var knowledgeBase = JsonConvert.DeserializeObject<KnowledgeBase>(File.ReadAllText(knowledgeBaseFile));
            return knowledgeBase;
        }

        private static void WriteRelationships(LibraryImporterConfigurationSection config, List<AlbumWithLocationAndTracks> allAlbums, List<ArtistWithTracks> allArtists)
        {
            Console.WriteLine("Writing track to album relations...");
            var relationsTrackAlbum = RelationsBuilder.GetRelationsTrackAlbum(allAlbums).ToList()
                .WriteFile(config.DataFiles.Relations.AlbumTrackFile);
            Console.WriteLine("Written {0} relations between tracks and albums.", relationsTrackAlbum.Count);

            Console.WriteLine("Writing composer to track relations...");
            var relationsComposerTrack = allArtists.SelectMany(RelationsBuilder.GetRelationsComposerTrack).ToList()
                .WriteFile(config.DataFiles.Relations.ComposerTrackFile);
            Console.WriteLine("Written {0} relations between composers and tracks.",
                relationsComposerTrack.Count);

            Console.WriteLine("Writing artist to track relations...");
            var relationsArtistTrack = allArtists.SelectMany(RelationsBuilder.GetRelationsArtistTrack).ToList()
                .WriteFile(config.DataFiles.Relations.ArtistTrackFile);
            Console.WriteLine("Extracted {0} relations between artists and tracks.",
                relationsArtistTrack.Count);

            Console.WriteLine("Writing featured artist relations to Tracks...");

            var relationsFeaturedArtistTrack = allArtists.SelectMany(RelationsBuilder.GetRelationsFeaturedArtistTrack).ToList()
                .WriteFile(config.DataFiles.Relations.FeaturedArtistTrackFile);
            Console.WriteLine("Written {0} relations to tracks from existing featured artists.",
                relationsFeaturedArtistTrack.Count);
        }

        private static void WriteNodes(LibraryImporterConfigurationSection config, ArtistNameExporter artistNameExporter,
            List<ArtistWithTracks> allArtists, List<AlbumWithLocationAndTracks> allAlbums, List<TrackWithFile> allTracks)
        {
            Console.WriteLine("Writing artist names...");
            var allArtistsWithRolesUnique = artistNameExporter.AggregateArtists(allArtists);
            allArtistsWithRolesUnique.WriteFile(config.DataFiles.Facts.AllArtistsFile);
            Console.WriteLine("Written {0} artist names.", allArtistsWithRolesUnique.Count);

            Console.WriteLine("Writing album names...");
            allAlbums.Select(a => (AlbumWithLocation) a).OrderBy(a => a.Name).ToList()
                .WriteFile(config.DataFiles.Facts.AllAlbumsFile);
            Console.WriteLine("Written {0} album names.", allAlbums.Count);

            Console.WriteLine("Writing track names...");
            allTracks.WriteFile(config.DataFiles.Facts.AllTracksFile);
            Console.WriteLine("Written {0} track names.", allTracks.Count);
        }

        private static ArtistNameExporter GetAllTrees(ArtistNameExporter artistNameExporter, List<FileWithTags> allFilesWithTags, LibraryImporterConfigurationSection config,
            List<ArtistWithTracks> allArtists, List<AlbumWithLocationAndTracks> allAlbums, List<TrackWithFile> allTracks)
        {
            Console.WriteLine("Extracting information from Tags...");

            artistNameExporter.Progress += NameExporter_TagPartRead;
            var done = 0;
            foreach (var tag in allFilesWithTags)
            {
                if (done % config.ReportFrequency == 0)
                    Console.WriteLine("{0} - Processing file: {1} of {2}", DateTime.Now, done, allFilesWithTags.Count);
                var trackWithFile = tag.ExtractTrack();
                allArtists.Merge(tag.ExtractArtists(artistNameExporter, trackWithFile));
                allAlbums.Merge(tag.ExtractAlbum(trackWithFile));
                allTracks.Add(trackWithFile);
                if (done++ % config.ReportFrequency == 0)
                    Console.WriteLine(
                        "Partial progress:\r\nArtists Found:\t{0}\r\nAlbum Artists Found:\t{1}\r\nComposers Found:\t{2}\r\nFeatured Artists:\t{3}\r\n",
                        Accumulator.ArtistsFound, Accumulator.AlbumArtistsFound,
                        Accumulator.ComposersFound, Accumulator.FeaturedArtistsFound);
            }

            Console.WriteLine(
                "Final progress:\r\nArtists Found:\t{0}\r\nAlbum Artists Found:\t{1}\r\nComposers Found:\t{2}\r\nFeatured Artists:\t{3}\r\n",
                Accumulator.ArtistsFound, Accumulator.AlbumArtistsFound,
                Accumulator.ComposersFound, Accumulator.FeaturedArtistsFound);
            return artistNameExporter;
        }

        private static List<string> _errorsInTags;
        private static List<FileWithTags> GetAllTags(bool rescan, LibraryImporterConfigurationSection config)
        {
            List<FileWithTags> allTags;
            if (rescan)
            {
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
            }
            else
            {
                Console.WriteLine("Loading tags from file: {0}...", config.DataFiles.NotProcessed.AllTagsFile);
                allTags = IoManager.ReadWithoutMapper<FileWithTags>(config.DataFiles.NotProcessed.AllTagsFile).ToList();
                Console.WriteLine("Loaded {0} tags.",allTags.Count);
            }

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

        private static void NameExporter_TagPartRead(object sender, ArtistNameExporterProgressEventArgs e)
        {
            Accumulator.AlbumArtistsFound += e.AlbumArtistsFound;
            Accumulator.ArtistsFound += e.ArtistsFound;
            Accumulator.ComposersFound += e.ComposersFound;
            Accumulator.FeaturedArtistsFound += e.FeaturedArtistsFound;

        }
    }
}
