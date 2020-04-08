using LIC.DataTypes;
using LIC.IoAccess;
using LIC.Mappers;
using Sciendo.ArtistClassifier.Contracts;
using Sciendo.ArtistClassifier.Contracts.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LIC
{
    public static class Extensions
    {
        public static TrackWithFile ExtractTrack(this FileWithTags input)
        {
            return new TrackWithFile
            {
                File = input.FilePath,
                Name = input.Title,
                TrackId = Guid.NewGuid(),
                TrackNo = input.Track,
                Year = input.Year
            };
        }

        public static TrackWithArtists ExtractArtists(this FileWithTags input,
    IArtistProcessor artistProcessor, TrackWithFile trackWithFile)
        {
            var trackWithArtistsWithRoles = new TrackWithArtists
            {
                File = trackWithFile.File,
                Name = trackWithFile.Name,
                TrackId = trackWithFile.TrackId,
                Artists = new List<Artist>(),
                TrackNo=trackWithFile.TrackNo,
                Year=trackWithFile.Year
            };
            if(!string.IsNullOrEmpty(input.Composers))
            {
                var composers = artistProcessor.GetArtists(input.Composers, false, true);
                if(composers!=null)
                    trackWithArtistsWithRoles.Artists.AddRange(composers);
            }
            if (!string.IsNullOrEmpty(input.Artists))
            {
                var artists = artistProcessor.GetArtists(input.Artists, false, false);
                if(artists!=null)
                    trackWithArtistsWithRoles.Artists.AddRange(artists);
            }
            if (!string.IsNullOrEmpty(input.AlbumArtists))
            {
                var albumArtists = artistProcessor.GetArtists(input.AlbumArtists, false, false);
                if(albumArtists!=null)
                    trackWithArtistsWithRoles.Artists.AddRange(albumArtists);
            }
            if (!string.IsNullOrEmpty(input.Title))
            {
                var featuredArtists = artistProcessor.GetArtists(input.Title, true, false);
                if(featuredArtists!=null)
                    trackWithArtistsWithRoles.Artists.AddRange(featuredArtists);
            }

            return trackWithArtistsWithRoles;
        }

        public static AlbumWithLocationAndTracks ExtractAlbum(this FileWithTags input, TrackWithFile trackWithFile)
        {
            return new AlbumWithLocationAndTracks
            {
                Name = input.Album,
                Location = Path.GetDirectoryName(input.FilePath)?.Trim().ToLower(),
                Tracks = new List<TrackWithFile> { trackWithFile }
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
                        ArtistId = Guid.NewGuid(),
                        Name = artist.Name,
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

        public static List<Artist> AggregateArtists(this List<ArtistWithTracks> allArtists)
        {
            return allArtists.Select(a => new Artist
            {
                ArtistId = a.ArtistId,
                Name = a.Name,
                Type = a.Tracks.First().Type,
                IsComposer = a.Tracks.Any(t => t.IsComposer),
                IsFeatured = a.Tracks.Any(t => t.IsFeatured)

            }).OrderBy(a => a.Name).ToList();
        }

        public static List<ArtistNode> MapToNodes(this List<Artist> input)
        {
            return input.Select(a => new ArtistNode { ArtistId = a.ArtistId, ArtistLabel = a.Type.ToString(), Name = a.Name }).ToList();
        }
        public static void WriteFile(this List<ArtistNode> input, string filePath)
        {
            IoManager.WriteWithMapper<ArtistNode, ArtistNodeMap>(input, filePath);
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

        public static List<RelationTrackAlbum> GetRelationsTrackAlbum(this List<AlbumWithLocationAndTracks> allAlbums)
        {
            List<RelationTrackAlbum> result = new List<RelationTrackAlbum>();
            allAlbums.ForEach(a => result.AddRange(a.Tracks.Select(t => new RelationTrackAlbum
            { AlbumId = a.AlbumId, TrackId = t.TrackId, TrackNo = t.TrackNo })));
            return result;
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

    }
}
