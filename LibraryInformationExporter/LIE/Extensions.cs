using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using LIE.DataTypes;
using LIE.Mappers;

namespace LIE
{
    public static class Extensions
    {
        public static List<FileWithTags> WriteFile(this IEnumerable<FileWithTags> input, string filePath)
        {
            IOManager.WriteWithoutMapper(input, filePath);
            return input.ToList();

        }

        public static void WriteFile(this List<string> input, string filePath)
        {
            if(input.Count>0)
                IOManager.WriteWithoutMapper(input, filePath);
        }


        public static void WriteFile(this List<Artist> input, string filePath)
        {
            IOManager.WriteWithMapper<Artist,ArtistWithRolesMap>(input, filePath);
        }

        public static List<AlbumWithLocation> WriteFile(this List<AlbumWithLocation> input, string filePath)
        {
            IOManager.WriteWithMapper<AlbumWithLocation,AlbumWithLocationMap>(input, filePath);
            return input;
        }

        public static List<TrackWithFile> WriteFile(this List<TrackWithFile> input, string filePath)
        {
            IOManager.WriteWithMapper<TrackWithFile,TrackWithFileMap>(input, filePath);
            return input;
        }

        public static List<RelationTrackAlbum> WriteFile(this List<RelationTrackAlbum> input, string filePath)
        {
            IOManager.WriteWithMapper<RelationTrackAlbum,RelationTrackAlbumMap>(input, filePath);
            return input;

        }

        public static List<RelationComposerTrack> WriteFile(this List<RelationComposerTrack> input, string filePath)
        {
            IOManager.WriteWithMapper<RelationComposerTrack,RelationComposerTrackMap>(input, filePath);
            return input;

        }

        public static List<RelationArtistTrack> WriteFile(this List<RelationArtistTrack> input, string filePath)
        {
            IOManager.WriteWithMapper<RelationArtistTrack,RelationArtistTrackMap>(input, filePath);
            return input;

        }

        public static TrackWithFile ExtractTrack(this FileWithTags input)
        {
            return new TrackWithFile {File = input.FilePath, Name = input.Title, TrackId = Guid.NewGuid(),TrackNo = input.Track};
        }
        public static TrackWithArtists ExtractArtists(this FileWithTags input,
            ArtistNameExporter artistNameExporter,TrackWithFile trackWithFile)
        {
            var trackWithArtistsWithRoles = new TrackWithArtists
            {
                File = trackWithFile.File, Name = trackWithFile.Name, TrackId = trackWithFile.TrackId,
                Artists = new List<Artist>()
            };

            artistNameExporter.AddComposers(input.Composers,trackWithArtistsWithRoles);
            artistNameExporter.AddArtists(input.Artists, trackWithArtistsWithRoles);
            artistNameExporter.AddAlbumArtists(input.AlbumArtists, trackWithArtistsWithRoles);
            artistNameExporter.AddFeaturedArtists(input.Title, trackWithArtistsWithRoles);

            return trackWithArtistsWithRoles;
        }

        public static AlbumWithLocationAndTracks ExtractAlbum(this FileWithTags input, TrackWithFile trackWithFile )
        {
            return new AlbumWithLocationAndTracks
            {
                Name = input.Album, Location = Path.GetDirectoryName(input.FilePath)?.Trim().ToLower(),
                Tracks = new List<TrackWithFile> {trackWithFile}
            };
        }

        public static void Merge(this List<ArtistWithTracks> to, TrackWithArtists from)
        {
            foreach (var artist in from.Artists)
            {
                var existingArtistWithRoles = to.FirstOrDefault(t =>
                    t.Name.Trim().ToLower() == artist.Name.Trim().ToLower());
                if (existingArtistWithRoles == null)
                {
                    to.Add(new ArtistWithTracks
                    {
                        ArtistId = Guid.NewGuid(), Name = artist.Name,
                        Tracks = new List<TrackByArtist>
                        {
                            new TrackByArtist
                            {
                                TrackId = from.TrackId, Type = artist.Type, File = from.File,
                                Name = from.Name, IsFeatured = artist.IsFeatured
                            }
                        }
                    });
                }
                else
                {
                    var existingTrack =
                        existingArtistWithRoles.Tracks.FirstOrDefault(t =>
                            t.File.ToLower().Trim() == from.File.ToLower().Trim());
                    if (existingTrack == null)
                    {
                        existingArtistWithRoles.Tracks.Add(new TrackByArtist
                        {
                            File = from.File,
                            Type = artist.Type,
                            IsFeatured = artist.IsFeatured,
                            Name = from.Name,
                            TrackId = from.TrackId
                        });
                    }
                }
            }
        }

        public static void Merge(this List<AlbumWithLocationAndTracks> to, AlbumWithLocationAndTracks from)
        {
            var existingAlbumWithLocation = to.FirstOrDefault(a => a.Location == from.Location);
            if (existingAlbumWithLocation == null)
            {
                from.AlbumId = Guid.NewGuid();
                to.Add(from);
                return;
            }

            var existingTrackInAlbumWithLocation =
                existingAlbumWithLocation.Tracks.FirstOrDefault(t => t.TrackId == from.Tracks[0].TrackId);
            if (existingTrackInAlbumWithLocation == null)
            {
                existingAlbumWithLocation.Tracks.Add(from.Tracks[0]);
                return;
            }

            return;
        }
        //public static List<ArtistWithRolesAndTracks> ExtractArtists(this List<FileWithTags> input,
        //    ArtistNameExporter artistNameExporter)
        //{
        //    return artistNameExporter.GetFullListOfArtistNamesFromTags(input);
        //}

        public static List<AlbumWithLocation> ExtractAlbums(this List<FileWithTags> input)
        {
            return input.Where(t => !String.IsNullOrEmpty(t.Album))
                .Select(f => new AlbumWithLocation
                {
                    AlbumId = Guid.NewGuid(),
                    Name = f.Album,
                    Location = Path.GetDirectoryName(f.FilePath)?.Trim().ToLower()
                }).Distinct(new AlbumWithLocationEqualityComparer()).OrderBy(a => a.Name).ToList();

        }

        public static List<TrackWithFile> ExtractTracks(this List<FileWithTags> input)
        {
            return input.Where(t => !String.IsNullOrEmpty(t.Title))
                .Select(f => new TrackWithFile
                {
                    TrackId = f.TrackId,
                    Name = f.Title.Trim(),
                    File = f.FilePath.Trim().ToLower()
                }).ToList();

        }

        public static bool IsNumber(this string inputString)
        {
            return Double.TryParse(inputString, out var temp);
        }


        //public static List<TrackWithFile> ExtractTracksWithPossibleFeaturedArtistNames(
        //    this List<FileWithTags> input)
        //{

        //    return new TracksToFeatureArtistNames().GetTracksWithPossibleFeaturedArtistNames(input).ToList();

        //}

        public static List<Artist> AddRangeAndReturn(this List<Artist> input,
            IEnumerable<Artist> add)
        {
            input.AddRange(add);
            return input;
        }
        public static List<RelationArtistTrack> AddRangeAndReturn(this List<RelationArtistTrack> input,
            IEnumerable<RelationArtistTrack> add)
        {
            input.AddRange(add);
            return input;
        }
    }
}
