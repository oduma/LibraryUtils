using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CommandLine;
using LIE.Configuration;
using LIE.DataTypes;
using LIE.Mappers;
using Sciendo.Common.IO;

namespace LIE
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            if (result.Tag == ParserResultType.Parsed)
            {
                var options = ((Parsed<Options>)result).Value;
                Console.WriteLine("Arguments Ok starting...");
                Console.WriteLine("reading configuration...");
                var config = ConfigurationManager.GetSection("libraryImporter") as LibraryImporterConfigurationSection;
                List<FileWithTags> allTags = GetAllTags(options.Rescan, config);
                if(options.Process)
                    ProcessTags(allTags, config);
                Console.WriteLine("Finished.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result));
        }

        private static ProgressAccumulator Accumulator= new ProgressAccumulator();
        private static void ProcessTags(List<FileWithTags> allFilesWithTags, 
            LibraryImporterConfigurationSection config)
        {
            //build a tree for artists
            List<ArtistWithTracks> allArtists = new List<ArtistWithTracks>();
            //build a tree for albums
            List<AlbumWithLocationAndTracks> allAlbums = new List<AlbumWithLocationAndTracks>();
            //build a list of tracks
            List<TrackWithFile> allTracks = new List<TrackWithFile>();
            Console.WriteLine("Extracting information from Tags...");

            ArtistNameExporter artistNameExporter = new ArtistNameExporter();
            artistNameExporter.Progress += NameExporter_TagPartRead;
            int done = 0;
                foreach (var tag in allFilesWithTags)
                {
                    if(done%config.ReportFrequency==0)
                        Console.WriteLine("{0} - Processing file: {1} of {2}",DateTime.Now,done,allFilesWithTags.Count);
                    var trackWithFile = tag.ExtractTrack();
                    allArtists.Merge(tag.ExtractArtists(artistNameExporter,trackWithFile));
                    allAlbums.Merge(tag.ExtractAlbum(trackWithFile));
                    allTracks.Add(trackWithFile);
                    if (done++ % config.ReportFrequency == 0)
                        Console.WriteLine(
                            "Done. Partial progress:\r\n Artists Found:\t{0}\r\nAlbum Artists Found:\t{1}\r\nComposers Found:\t{2}\r\nFeatured Artists:\t{3}\r\n",
                            Accumulator.ArtistsFound, Accumulator.AlbumArtistsFound,
                            Accumulator.ComposersFound, Accumulator.FeaturedArtistsFound);
                }

                Console.WriteLine("Writing artist names...");
                var allArtistsWithRolesUnique = artistNameExporter.AggregateArtists(allArtists);
                allArtistsWithRolesUnique.WriteFile(config.DataFiles.Facts.AllArtistsFile);
                Console.WriteLine("Written {0} artist names.", allArtistsWithRolesUnique.Count);

                Console.WriteLine("Writing album names...");
                allAlbums.Select(a=>(AlbumWithLocation)a).ToList().WriteFile(config.DataFiles.Facts.AllAlbumsFile);
                Console.WriteLine("Written {0} album names.", allAlbums.Count);

                Console.WriteLine("Writing track names...");
                allTracks.WriteFile(config.DataFiles.Facts.AllTracksFile);
                Console.WriteLine("Written {0} track names.", allTracks.Count);

                var relationsTrackAlbum = new List<RelationTrackAlbum>();
                var relationsComposerTrack = new List<RelationComposerTrack>();
                var relationsArtistTrack = new List<RelationArtistTrack>();

                Console.WriteLine("Writing track to album relations...");
                relationsTrackAlbum = RelationsBuilder.GetRelationsTrackAlbum(allAlbums).ToList()
                    .WriteFile(config.DataFiles.Relations.AlbumTrackFile);
                Console.WriteLine("Written {0} relations between tracks and albums.", relationsTrackAlbum.Count);

                Console.WriteLine("Writing composer to track relations...");
                relationsComposerTrack =allArtists.SelectMany(RelationsBuilder.GetRelationsComposerTrack).ToList()
                    .WriteFile(config.DataFiles.Relations.ComposerTrackFile);
                Console.WriteLine("Written {0} relations between composers and tracks.",
                    relationsComposerTrack.Count);

                Console.WriteLine("Writing artist to track relations...");
                relationsArtistTrack = allArtists.SelectMany(RelationsBuilder.GetRelationsArtistTrack).ToList()
                    .WriteFile(config.DataFiles.Relations.ArtistTrackFile);
                Console.WriteLine("Extracted {0} relations between artists and tracks.",
                    relationsArtistTrack.Count);

            //if (featured)
            //{
                Console.WriteLine("Writing featured artist relations to Tracks...");

                List<RelationArtistTrack> relationsFeaturedArtistTrack = new List<RelationArtistTrack>();
                relationsFeaturedArtistTrack =allArtists.SelectMany(RelationsBuilder.GetRelationsFeaturedArtistTrack).ToList().WriteFile(config.DataFiles.Relations.FeaturedArtistTrackFile);
                Console.WriteLine("Written {0} relations to tracks from existing featured artists.",
                    relationsFeaturedArtistTrack.Count);

            //    Console.WriteLine("Extracting new artist names from featured artists...");
            //    List<Artist> newArtists = new List<Artist>();
            //    newArtists = new ArtistNameExporter().GetArtistsWithRoles(featuredArtistsNotFound.Select(f =>
            //    new Artist
            //    { Name = f.Name, Type = ArtistType.Artist }));
            //    Console.WriteLine("Extracted {0} new artist names from featured artists.", newArtists.Count);


            //    Console.WriteLine("Establishing relations between new artist names and tracks...");
            //    List<RelationArtistTrack> newArtistsWithTracks = new List<RelationArtistTrack>();
            //    newArtistsWithTracks =
            //    RelationsBuilder.GetRelationsFeaturedNewArtistTrack(allTags, newArtists, featuredArtistsNotFound);
            //    Console.WriteLine("Established {0} relations between new artist names and tracks.", newArtistsWithTracks.Count);

            //    allArtists.AddRangeAndReturn(newArtists).WriteFile(config.DataFiles.Facts.AllArtistsFile);
            //    relationsArtistTrack.AddRangeAndReturn(relationsFeaturedArtistTrack)
            //        .AddRangeAndReturn(newArtistsWithTracks).WriteFile(config.DataFiles.Relations.ArtistTrackFile);

            //    Console.WriteLine("Saved {0} artists and {1} relations between artists and tracks",
            //        allArtists.Count, relationsArtistTrack.Count);
            //}


            //}
            //else
            //{
            //    Console.WriteLine("Loading track to album relations from file {0}...", config.DataFiles.Relations.AlbumTrackFile);
            //    relationsTrackAlbum =
            //        IOManager.ReadWithMapper<RelationTrackAlbum, RelationTrackAlbumMap>(config.DataFiles.Relations.AlbumTrackFile);
            //    Console.WriteLine("Loaded {0} relations between tracks and albums.", relationsTrackAlbum.Count);

            //    Console.WriteLine("Loading composer to track relations from file {0}...", config.DataFiles.Relations.ComposerTrackFile);
            //    relationsComposerTrack = IOManager.ReadWithMapper<RelationComposerTrack, RelationComposerTrackMap>(config.DataFiles.Relations.ComposerTrackFile);
            //    Console.WriteLine("Loaded {0} relations between composers and tracks.",
            //        relationsComposerTrack.Count);

            //    Console.WriteLine("Loading artist to track relations from file {0}...", config.DataFiles.Relations.ArtistTrackFile);
            //    relationsArtistTrack = IOManager.ReadWithMapper<RelationArtistTrack, RelationArtistTrackMap>(config.DataFiles.Relations.ArtistTrackFile);
            //    Console.WriteLine("Extracted {0} relations between artists and tracks.",
            //        relationsArtistTrack.Count);
            //}

            //}
            //else
            //{
            //    Console.WriteLine("Loading artist names from file{0}...", config.DataFiles.Facts.AllArtistsFile);
            //    allArtists = IOManager.ReadWithMapper<ArtistWithRoles,ArtistWithRolesMap>(config.DataFiles.Facts.AllArtistsFile);
            //    Console.WriteLine("Loaded {0} artist names.", allArtists.Count);

            //    Console.WriteLine("Loading album names from file{0}...", config.DataFiles.Facts.AllAlbumsFile);
            //    allAlbums = IOManager.ReadWithMapper<AlbumWithLocation,AlbumWithLocationMap>(config.DataFiles.Facts.AllAlbumsFile);
            //    Console.WriteLine("Loaded {0} album names.", allAlbums.Count);

            //    Console.WriteLine("Loading tracks from file{0}...", config.DataFiles.Facts.AllTracksFile);
            //    allTracks = IOManager.ReadWithMapper<TrackWithFile,TrackWithFileMap>(config.DataFiles.Facts.AllTracksFile);
            //    Console.WriteLine("Loaded {0} tracks.", allTracks.Count);

            //}
        }

        private static List<string> _errorsInTags;
        private static List<FileWithTags> GetAllTags(bool rescan, LibraryImporterConfigurationSection config)
        {
            List<FileWithTags> allTags= new List<FileWithTags>();
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
                allTags = IOManager.ReadWithoutMapper<FileWithTags>(config.DataFiles.NotProcessed.AllTagsFile).ToList();
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

        private static void TagsProvider_TagPartRead(object sender, TagProviderProgressEventArgs e)
        {
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
