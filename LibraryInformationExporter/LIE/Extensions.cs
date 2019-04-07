using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LIE.DataTypes;
using LIE.Mappers;

namespace LIE
{
    public static class Extensions
    {
        public static List<FileWithTags> WriteFile(this IEnumerable<FileWithTags> input, string filePath)
        {
            IoManager.WriteWithoutMapper(input, filePath);
            return input.ToList();

        }

        public static void WriteFile(this List<string> input, string filePath)
        {
            if (input.Count > 0)
                IoManager.WriteWithMapper<string, StringMap>(input, filePath);
        }


        public static void WriteFile(this List<Artist> input, string filePath)
        {
            IoManager.WriteWithMapper<Artist, ArtistMap>(input, filePath);
        }

        public static List<AlbumWithLocation> WriteFile(this List<AlbumWithLocation> input, string filePath)
        {
            IoManager.WriteWithMapper<AlbumWithLocation, AlbumWithLocationMap>(input, filePath);
            return input;
        }

        public static List<TrackWithFile> WriteFile(this List<TrackWithFile> input, string filePath)
        {
            IoManager.WriteWithMapper<TrackWithFile, TrackWithFileMap>(input, filePath);
            return input;
        }

        public static List<RelationTrackAlbum> WriteFile(this List<RelationTrackAlbum> input, string filePath)
        {
            IoManager.WriteWithMapper<RelationTrackAlbum, RelationTrackAlbumMap>(input, filePath);
            return input;

        }

        public static List<RelationComposerTrack> WriteFile(this List<RelationComposerTrack> input, string filePath)
        {
            IoManager.WriteWithMapper<RelationComposerTrack, RelationComposerTrackMap>(input, filePath);
            return input;

        }

        public static List<RelationArtistTrack> WriteFile(this List<RelationArtistTrack> input, string filePath)
        {
            IoManager.WriteWithMapper<RelationArtistTrack, RelationArtistTrackMap>(input, filePath);
            return input;

        }

        public static TrackWithFile ExtractTrack(this FileWithTags input)
        {
            return new TrackWithFile
            {
                File = input.FilePath, Name = input.Title, TrackId = Guid.NewGuid(), TrackNo = input.Track,
                Year = input.Year
            };
        }

        public static TrackWithArtists ExtractArtists(this FileWithTags input,
            ArtistNameExporter artistNameExporter, TrackWithFile trackWithFile)
        {
            var trackWithArtistsWithRoles = new TrackWithArtists
            {
                File = trackWithFile.File, Name = trackWithFile.Name, TrackId = trackWithFile.TrackId,
                Artists = new List<Artist>()
            };

            artistNameExporter.AddComposers(input.Composers, trackWithArtistsWithRoles);
            artistNameExporter.AddArtists(input.Artists, trackWithArtistsWithRoles);
            artistNameExporter.AddAlbumArtists(input.AlbumArtists, trackWithArtistsWithRoles);
            artistNameExporter.AddFeaturedArtists(input.Title, trackWithArtistsWithRoles);

            return trackWithArtistsWithRoles;
        }

        public static string ReplaceAll(this string input, IEnumerable<string> regexFind, string withString)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            string result = input;
            foreach (var regexFindPart in regexFind)
            {
                var possibleMatches = Regex.Matches(input, regexFindPart);
                if (possibleMatches.Count > 0)
                {
                    foreach (var possibleMatch in possibleMatches)
                    {
                        result = result.Replace(possibleMatch.ToString(), withString);
                    }
                }
            }

            return result;
        }

        public static AlbumWithLocationAndTracks ExtractAlbum(this FileWithTags input, TrackWithFile trackWithFile)
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
                                Name = from.Name, IsFeatured = artist.IsFeatured, IsComposer = artist.IsComposer
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
                            IsComposer = artist.IsComposer,
                            Name = from.Name,
                            TrackId = from.TrackId
                        });
                    }
                    else
                    {
                        if (existingTrack.IsComposer == false)
                        {
                            existingTrack.IsComposer = artist.IsComposer;
                        }

                        if (existingTrack.IsFeatured == false)
                        {
                            existingTrack.IsFeatured = artist.IsFeatured;
                        }
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
                existingAlbumWithLocation.Tracks.Add(from.Tracks[0]);
        }
    }
}
