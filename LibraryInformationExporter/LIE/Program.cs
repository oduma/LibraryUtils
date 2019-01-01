using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using LIE.DataTypes;
using LIE.Mappers;
using Sciendo.Common.IO;

namespace LIE
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting ...");
            string allTagsFile = "allFilesWithTags.csv";
            var filesCollector = new FilesCollector(new FsStorage());
            filesCollector.Progress += NameExporter_TagPartRead;
            if (args[0].ToLower() == "rescan")
            {
                CollectionOfFiles(filesCollector, @"c:\users\octo\Music\m\Michael Jackson\",
                    new[] { ".mp3", ".flac", ".wma", ".m4a" }, allTagsFile);
            }
            Console.WriteLine("Loading tags from file: {0}...", allTagsFile);
            List<FileWithTags> allTags = filesCollector
                .GetFullListOfTagsFromFile(allTagsFile);
            Console.WriteLine("Loaded {0} files with tags.",allTags.Count);

            List<ArtistWithRoles> allArtists;
            List<AlbumWithLocation> allAlbums;
            if (args[0].ToLower() != "relationsonly")
            {
                allArtists = LoadAllArtistNamesFromTags(allTags);
                allAlbums = LoadAllAlbumsFromTags(allTags);
                LoadAllTracksFromTags(allTags);
            }
            else
            {
                var allArtistsFile = "allArtistNames.csv";
                var allAlbumsFile = "allAlbumNames.csv";
                Console.WriteLine("Loading artist names from file{0}...", allArtistsFile);
                allArtists = FilesCollector.LoadAllArtistNamesFromFile(allArtistsFile);
                Console.WriteLine("Loading album names from file{0}...", allAlbumsFile);
                allAlbums = FilesCollector.LoadAllAlbumNamesFromFile(allAlbumsFile);
            }
            EstablishRelations(allTags, allAlbums, allArtists);
            Console.WriteLine("finished.");
            Console.ReadKey();
        }

        private static void EstablishRelations(List<FileWithTags> allTags,List<AlbumWithLocation> allAlbums, List<ArtistWithRoles> allArtists)
        {
            Console.WriteLine("Creating relations...");
            Console.WriteLine("Writing track to album relations to file RelationTrackAlbum.csv...");

            using (var writer = new StreamWriter("RelationTrackAlbum.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<RelationTrackAlbumMap>();
                csv.WriteRecords(RelationsBuilder.GetRelationsTrackAlbum(allTags,allAlbums));
            }
            Console.WriteLine("Writing composer to track relations to file RelationComposerTrack.csv...");
            using (var writer = new StreamWriter("RelationComposerTrack.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<RelationComposerTrackMap>();
                csv.WriteRecords(RelationsBuilder.GetRelationsComposerTrack(allTags,
                    allArtists.Where(a => a.ArtistRoles.Contains(ArtistRole.Composer)).Select(a => a)));

            }
            Console.WriteLine("Writing artist to track relations to file RelationArtistTrack.csv...");
            using (var writer = new StreamWriter("RelationArtistTrack.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<RelationArtistTrackMap>();
                csv.WriteRecords(RelationsBuilder.GetRelationsArtistTrack(allTags,allArtists));
            }
        }

        private static void LoadAllTracksFromTags(List<FileWithTags> allTags)
        {
            Console.WriteLine("Loading track names from Tags...");
            IEnumerable<TrackWithFile> tracksWithFiles=allTags.Where(t => !string.IsNullOrEmpty(t.Title))
                .Select(f => new TrackWithFile
            {
                TrackId = f.TrackId,
                Name = f.Title.Trim(),
                File = f.FilePath.Trim().ToLower()
            });

            using (var writer = new StreamWriter("allTrackNames.csv"))
                using(var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<TrackWithFileMap>();
                csv.WriteRecords(tracksWithFiles);
            }
        }

        private static List<AlbumWithLocation> LoadAllAlbumsFromTags(List<FileWithTags> allTags)
        {
            Console.WriteLine("Loading album names from Tags...");
            List<AlbumWithLocation> albumsWithLocations= allTags.Where(t => !string.IsNullOrEmpty(t.Album))
                .Select(f => new AlbumWithLocation
            {
                AlbumId = Guid.NewGuid(),
                Name = f.Album,
                Location = Path.GetDirectoryName(f.FilePath)?.Trim().ToLower()
            }).Distinct(new AlbumWithLocationEqualityComparer()).OrderBy(a => a.Name).ToList();
            using (var writer = new StreamWriter("allAlbumNames.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<AlbumWithLocationMap>();
                csv.WriteRecords(albumsWithLocations);
            }
            return albumsWithLocations;
        }

        private static List<ArtistWithRoles> LoadAllArtistNamesFromTags(
            List<FileWithTags> allTags)
        {

            Console.WriteLine("Loading artist names from Tags...");
            ArtistNameExporter artistNameExporter= new ArtistNameExporter();
            artistNameExporter.Progress += NameExporter_TagPartRead;
            var allArtistsFile = "allArtistNames.csv";
            List<ArtistWithRoles> artistWithRoles = artistNameExporter
                .GetFullListOfArtistNamesFromTags(allTags);
            using (var writer = new StreamWriter(allArtistsFile))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<ArtistWithRolesMap>();
                csv.WriteRecords(artistWithRoles);
            }
            return artistWithRoles;
        }
        private static void CollectionOfFiles(FilesCollector filesCollector, string path, string[] extensions, string allTagsFile)
        {
            using (var writer = new StreamWriter(allTagsFile))
            using (var csv = new CsvWriter(writer))
                csv.WriteRecords(filesCollector.ScanPath(path, extensions).Where(t => t != null));
        }
        private static void NameExporter_TagPartRead(object sender, ProgressEventArgs e)
        {
            Console.WriteLine("Reading and preprocessing: {0} .", e.Message);
        }
    }
}
