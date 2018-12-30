using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LIE
{
    internal static class RelationsBuilder
    {
        public static RelationTrackAlbum GetRelationTrackAlbum(TrackWithFile track, List<FileWithTag> currentTrackTags, List<AlbumWithLocation> allAlbums)
        {
            var tagAlbum = currentTrackTags.FirstOrDefault(t => t.TagType == TagType.Album);
            if (tagAlbum == null)
                return null;
            var tagTrackNo = currentTrackTags.FirstOrDefault((t => t.TagType == TagType.Track));

            var currentAlbum = allAlbums.FirstOrDefault(a =>
                a.Name.Trim().ToLower() == tagAlbum.TagContents.Trim().ToLower() && a.Location.Trim().ToLower() ==
                Path.GetDirectoryName(tagAlbum.FilePath).Trim().ToLower());

            if (currentAlbum == null)
                return null;

            return new RelationTrackAlbum
            {
                TrackId = track.TrackId, TrackNo = (tagTrackNo == null) ? string.Empty : tagTrackNo.TagContents,
                AlbumId = currentAlbum.AlbumId
            };
        }

        public static IEnumerable<RelationComposerTrack> GetRelationComposerTrack(TrackWithFile track, List<FileWithTag> currentTrackTags, List<ArtistWithRoles> allArtists)
        {
            var tagComposers = currentTrackTags.FirstOrDefault(t => t.TagType == TagType.Composers);
            if (tagComposers != null)
            {
                ArtistNameExporter artistNameExporter = new ArtistNameExporter();
                var composersNames = artistNameExporter.ComposersDisambiguated(tagComposers.TagContents.Split(new[] { ';' }));
                var composers = allArtists.Where(a => composersNames.Any(c => c.Name.Trim().ToLower() == a.Name.Trim().ToLower()));
                foreach (var composer in composers)
                {
                    yield return new RelationComposerTrack { TrackId = track.TrackId, ArtistId = composer.ArtistId };
                }
            }
        }

        public static IEnumerable<RelationArtistTrack> GetRelationArtistTrack(TrackWithFile track, List<FileWithTag> currentTrackTags, List<ArtistWithRoles> allArtists)
        {
            var tagsArtists =
                currentTrackTags.Where(t => t.TagType == TagType.Artist || t.TagType == TagType.AlbumArtist).ToList();
            var year = currentTrackTags.FirstOrDefault(t => t.TagType == TagType.Year);
            if (tagsArtists.Count>0)
            {
                ArtistNameExporter artistNameExporter = new ArtistNameExporter();
                var artists = artistNameExporter.GetFullListOfArtistNamesFromTags(tagsArtists);
                var existingArtists= allArtists.Where(a => artists.Any(c => c.Name.Trim().ToLower() == a.Name.Trim().ToLower()));
                foreach (var existingArtist in existingArtists)
                {
                    yield return new RelationArtistTrack { TrackId = track.TrackId, ArtistId = existingArtist.ArtistId, Year = (year==null)?String.Empty : year.TagContents};
                }

            }

        }
    }
}