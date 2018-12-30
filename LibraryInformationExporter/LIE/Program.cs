using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;

namespace LIE
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting ...");
            var fsStorage = new FsStorage();
            string allTagsFile = "allFilesWithTags.tsv";
            var alltagsTitleLine = "File\tTagType\tTag";
            List<FileWithTag> allTags;
            if (args[0].ToLower() == "rescan")
            {
                allTags= CollectionOfFiles(fsStorage, @"c:\users\octo\Music\",
                    new[] { ".mp3", ".flac", ".wma", ".m4a" }, alltagsTitleLine, allTagsFile);

            }
            else
            {
                Console.WriteLine("Loading tags from file: {0}...", allTagsFile);
                allTags= FilesCollector
                    .GetFullListOfTagsFromFile(allTagsFile, alltagsTitleLine);
            }

            List<ArtistWithRoles> allArtists;
            List<AlbumWithLocation> allAlbums;
            List<TrackWithFile> allTracks;
            var artistTitleLine = "artistID:ID(Artist),name,:LABEL";
            var albumTitleLine = "albumID:ID(Album),name,Location";
            var trackTitleLine = "trackID:ID(Track),name,File";
            if (args[0].ToLower() != "relationsonly")
            {
                allArtists = LoadAllArtistNamesFromTags(artistTitleLine, allTags);
                allAlbums = LoadAllAlbumsFromTags(albumTitleLine, allTags);
                allTracks = LoadAllTracksFromTags(trackTitleLine, allTags);
            }
            else
            {
                var allArtistsFile = "allArtistNames.csv";
                var allAlbumsFile = "allAlbumNames.csv";
                var allTracksFile = "allTrackNames.csv";
                Console.WriteLine("Loading track names from file{0}...",allTracksFile);
                allTracks = FilesCollector.LoadAllTrackNamesFromFile(allTracksFile);
                Console.WriteLine("Loading artist names from file{0}...", allArtistsFile);
                allArtists = FilesCollector.LoadAllArtistNamesFromFile(allArtistsFile);
                Console.WriteLine("Loading album names from file{0}...", allAlbumsFile);
                allAlbums = FilesCollector.LoadAllAlbumNamesFromFile(allAlbumsFile);
            }
            EstablishRelations(allTags, allTracks, allAlbums, allArtists);
            Console.WriteLine("finished.");
            Console.ReadKey();
        }


        private static List<AlbumWithLocation> LoadAllAlbumNamesFromFile(string allAlbumsFile, string albumTitleLine)
        {
            throw new NotImplementedException();
        }


        private static void EstablishRelations(List<FileWithTag> allTags,List<TrackWithFile> allTracks, List<AlbumWithLocation> allAlbums, List<ArtistWithRoles> allArtists)
        {
            Console.WriteLine("Creating relations...");
            var rATTitleLine = ":START_ID(Artist),year,:END_ID(Track)";
            var rCTTitleLine = ":START_ID(Artist),:END_ID(Track)";
            var rTATitleLine = ":START_ID(Track),track_no,:END_ID(Album)";
            using (var fsRAT = File.CreateText("RelationArtistTrack.csv"))
            {
                Console.WriteLine("Writing artist to track relations to file RelationArtistTrack.csv...");
                fsRAT.WriteLine(rATTitleLine);
                using (var fsRCT = File.CreateText("RelationComposerTrack.csv"))
                {
                    Console.WriteLine("Writing composer to track relations to file RelationComposerTrack.csv...");
                    fsRCT.WriteLine(rCTTitleLine);
                    using (var fsRTA = File.CreateText("RelationTrackAlbum.csv"))
                    {
                        Console.WriteLine("Writing track to album relations to file RelationTrackAlbum.csv...");
                        fsRTA.WriteLine(rTATitleLine);
                        foreach (var track in allTracks)
                        {
                            if(track.TrackId.EndsWith("0"))
                                Console.WriteLine("Processed tracks to {0} for relations...",track.TrackId);
                            List<FileWithTag> currentTrackTags =
                                allTags.Where(t => t.FilePath.ToLower() == track.File.ToLower()).ToList();
                            RelationTrackAlbum rta =
                                RelationsBuilder.GetRelationTrackAlbum(track, currentTrackTags, allAlbums);
                            if(rta!=null)
                                fsRTA.WriteLine($"{rta.TrackId},{rta.TrackNo},{rta.AlbumId}");
                            foreach (RelationComposerTrack rct in
                                RelationsBuilder.GetRelationComposerTrack(track, currentTrackTags, allArtists))
                            {
                                fsRCT.WriteLine($"{rct.ArtistId},{rct.TrackId}");
                            }

                            foreach (RelationArtistTrack rat in
                                RelationsBuilder.GetRelationArtistTrack(track, currentTrackTags, allArtists))
                            {
                                fsRAT.WriteLine($"{rat.ArtistId},{rat.Year},{rat.TrackId}");
                            }

                        }
                    }
                }
            }
        }

        private static List<TrackWithFile> LoadAllTracksFromTags(string targetTitleLine, List<FileWithTag> allTags)
        {
            Console.WriteLine("Loading track names from Tags...");
            List<TrackWithFile> tracksWithFiles;
            using (var fs = File.CreateText("allTrackNames.csv"))
            {
                fs.WriteLine(targetTitleLine);
                int i = 1;
                tracksWithFiles = allTags.Where(t => t.TagType == TagType.Title).Select(f => new TrackWithFile
                {
                    Name = $@"""{f.TagContents.Trim().Replace(@"""", @"""""")}""",
                    File = $@"""{f.FilePath.ToLower().Replace(@"""", @"""""")}"""
                }).Distinct(new TrackWithFileEqualityComparer()).OrderBy(t => t.Name).ToList();
                foreach (var track in tracksWithFiles)
                {
                    track.TrackId = $"tra{i++}";
                    fs.WriteLine(
                        $"{track.TrackId},{track.Name},{track.File}");
                }

            }

            return tracksWithFiles;
        }

        private static List<AlbumWithLocation> LoadAllAlbumsFromTags(string targetTitleLine, List<FileWithTag> allTags)
        {
            Console.WriteLine("Loading album names from Tags...");
            List<AlbumWithLocation> albumsWithLocations= allTags.Where(t => t.TagType == TagType.Album).Select(f => new AlbumWithLocation
            {
                Name = $@"""{f.TagContents.Replace(@"""", @"""""")}""",
                Location = $@"""{Path.GetDirectoryName(f.FilePath).ToLower().Replace(@"""", @"""""")}"""
            }).Distinct(new AlbumWithLocationEqualityComparer()).OrderBy(a => a.Name).ToList();
            using (var fs = File.CreateText("allAlbumNames.csv"))
            {
                fs.WriteLine(targetTitleLine);
                int i = 1;
                foreach (var album in albumsWithLocations)
                {
                    album.AlbumId = $"alb{i++}";
                    fs.WriteLine(
                        $"{album.AlbumId},{album.Name},{album.Location}");
                }
            }
            return albumsWithLocations;
        }

            private static void ValidateOneWordArtists(string titleLine)
        {
            using (var fs = File.CreateText("1WordArtistNames.csv"))
            {
                fs.WriteLine(titleLine);
                foreach (var inLine in File.ReadAllLines("allArtistNames.csv"))
                {
                    if (inLine != titleLine && !string.IsNullOrEmpty(inLine))
                    {
                        var inLineParts = inLine.Split(new[] {','});
                        if (inLineParts[1].Split(new []{' '}).Length==1)
                            fs.WriteLine(inLine);
                    }
                }
            }
        }

        private static List<ArtistWithRoles> LoadAllArtistNamesFromTags(string targetTitleLine,
            List<FileWithTag> allTags)
        {

            Console.WriteLine("Loading artist names from Tags...");
            ArtistNameExporter artistNameExporter= new ArtistNameExporter();
            artistNameExporter.ArtistRead += NameExporter_TagPartRead;
            List<ArtistWithRoles> artistsWithRoles;
            var allArtistsFile = "allArtistNames.csv";
            using (var fs = File.CreateText(allArtistsFile))
            {
                fs.WriteLine(targetTitleLine);
                int i = 1;
                artistsWithRoles = artistNameExporter
                    .GetFullListOfArtistNamesFromTags(allTags);
                foreach (var artistWithRoles in artistsWithRoles)
                {
                    if(artistWithRoles!=null)
                    {
                        artistWithRoles.ArtistId = $"art{i++}";
                        fs.WriteLine(
                            $"{artistWithRoles.ArtistId},{(artistWithRoles.ProcessedName) ?? artistWithRoles.Name},{string.Join(";", artistWithRoles.ArtistRoles)}");
                    }
                }

            }

            return artistsWithRoles;
        }
        private static void LoadPreviousArtistNames(FsStorage fsStorage, string titleLine, List<ArtistWithRole> previousArtistNames)
        {
            foreach (var previousFileName in fsStorage.Directory.GetFiles(".", SearchOption.TopDirectoryOnly))
            {
                if (previousFileName.Contains("artistNames") && !previousFileName.Contains("artistNames.csv"))
                {
                    Console.WriteLine("Loading artist names from file: {0}...", previousFileName);
                    foreach (var line in File.ReadAllLines(previousFileName))
                    {
                        if (line != titleLine)
                        {
                            var splitLineParts = line.Split(new char[] {','});
                            previousArtistNames.Add(new ArtistWithRole
                                {ArtistId = splitLineParts[0], Name = splitLineParts[1]});
                            //Console.WriteLine("Previous Artist Name Loaded: {0}", splitLineParts[1]);
                        }
                    }
                }
            }
        }

        private static List<FileWithTag> CollectionOfFiles(FsStorage fsStorage, string path, string[] extensions, string titleLine, string allTagsFile)
        {
            List<FileWithTag> filesWithTags;
            using (var fs = File.CreateText(allTagsFile))
            {
                fs.WriteLine(titleLine);
                var filesCollector = new FilesCollector();
                filesCollector.FolderRead += NameExporter_TagPartRead;
                filesWithTags = filesCollector.ScanFolder(fsStorage, path, extensions);
                foreach (var fileWithTags in  filesWithTags)
                {
                    fs.WriteLine($"{fileWithTags.FilePath}\t{fileWithTags.TagType}\t{fileWithTags.TagContents}");
                }
            }

            return filesWithTags;
        }

        private static void Pass2Aggregation(List<ArtistWithRole> previousArtistNames, string titleLine)
        {
            using (var fs = File.CreateText("allArtistNames.csv"))
            {
                fs.WriteLine(titleLine);
                foreach (var artistWithRole in previousArtistNames.OrderBy(a => a.Name))
                {
                    fs.WriteLine($"{artistWithRole.ArtistId},{artistWithRole.Name},{ArtistRole.Artist}");
                }
            }
        }

        private static void Pass1Collection(FsStorage fsStorage, List<ArtistWithRole> previousArtistNames, string titleLine)
        {
            ArtistNameExporter artistNameExporter = new ArtistNameExporter(fsStorage, previousArtistNames);
            artistNameExporter.ArtistRead += NameExporter_TagPartRead;
            using (var fs = File.CreateText("artistNames.csv"))
            {
                fs.WriteLine(titleLine);
                //0-9 started with 1 finished with 49
                //a started with 50 finished with 411
                //b started with 412 finished with 984
                //c started with 985 finished with 1407
                //clasic-compilations started with 1408 finished with 1490
                //comedy-others started with 1491 finished with 1637
                //country comiplations started with 1638 finished with 1844
                //d started with 1845 finished with 2238
                //e started with 2239 finished with 2486
                //dance compilations started with 2487 finished with 3706
                //f started with 3707 finished with 3920
                //folk compilations started with 3721 finished with 3741
                //g started with 3742 finished with 3972
                //h started with 3973 finished with 4098
                //i started with 4099 finished with 4151
                //j started with 4152 finished with 4421
                //jazz compilations started with 4422 finished with 5660
                //k started with 5661 finished with 5931
                //l started with 5932 finished with 6204
                //m started with 6205 finished with 6766
                //n started with 6767 finished with 7016
                //o started with 7017 finished with 7106
                //oldies comiplations started with 7107 finished with 7472
                //p started with 7473 finished with 7720
                //pop comiplations started with 7721 finished with 9829
                //q started with 9830 finished with 9856
                //r started with 9857 finished with 10137
                //reggae comiplations started with 10138 finished with 10207
                //rock compilations started with 10208 finished with 20408
                //s started with 20409 finished with  20851
                //sountracks started with 20852 finished with 21461
                //t started with 21462 finished with 21688
                //u started with 21689 finished with 21703
                //v started with 21704 finished with 21770
                //w started with 21771 finished with 21857
                //x started with 21858 finished with 21864
                //y started with 21865 finished with 21880
                //z started with 21881 finished with 21927
                int i = 21881;
                List<string> notWrittenArtists = new List<string>();
                foreach (var artistWithRole in artistNameExporter.GetFullListOfArtistNames(@"c:\users\octo\Music\z\",
                    new[] {".mp3", ".flac", ".wma", ".m4a"}).Distinct(new ArtistWithRoleEqualityComparer()))
                {
                    //var processedArtistWithRole = artistNameExporter.Process(artistWithRole);
                    var processedArtistWithRole = artistWithRole;
                    var artistName = (processedArtistWithRole.ProcessedName) ?? processedArtistWithRole.Name;
                    bool prevLoaded = artistNameExporter.ArtistPreviouslyLoaded(artistName);
                    if (!prevLoaded)
                    {
                        Console.WriteLine("{2}. Writing: {0} as a {1}.", artistName, ArtistRole.Artist, i);
                        fs.WriteLine($"art{i++},{artistName},{ArtistRole.Artist}");
                    }
                    else
                    {
                        Console.WriteLine("Previously known artist:{0}", artistName);
                        notWrittenArtists.Add(artistName);
                    }
                }

                foreach (var notWrittenArtist in notWrittenArtists)
                {
                    Console.WriteLine("Not Written: {0}", notWrittenArtist);
                }
            }
        }

        private static void NameExporter_TagPartRead(object sender, TagPartReadEventArgs e)
        {
            Console.WriteLine("Reading and preprocessing: {0} .", e.Name);
        }
    }
}
