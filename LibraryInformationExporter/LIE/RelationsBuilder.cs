using System.Collections.Generic;
using System.IO;
using System.Linq;
using LIE.DataTypes;

namespace LIE
{
    internal static class RelationsBuilder
    {
        public static IEnumerable<RelationTrackAlbum> GetRelationsTrackAlbum(List<FileWithTags> allTags, List<AlbumWithLocation> allAlbums)
        {
            var q = from album in allAlbums
                join tag in allTags
                    on album.Location.Trim().ToLower() equals Path.GetDirectoryName(tag.FilePath)?.Trim().ToLower()
                select new RelationTrackAlbum {AlbumId = album.AlbumId, TrackId = tag.TrackId, TrackNo = tag.Track};

            return q;
        }

        public static IEnumerable<RelationComposerTrack> GetRelationsComposerTrack(List<FileWithTags> allTags, IEnumerable<ArtistWithRoles> allArtists)
        {
            ArtistNameExporter artistNameExporter = new ArtistNameExporter();
            var q = from artist in allArtists
                from tag in allTags
                    where artistNameExporter.ComposersDisambiguated(tag.Composers.Split(';')).Select(a=>a.Name.Trim().ToLower()).Contains(artist.Name.Trim().ToLower())
                select new RelationComposerTrack { ArtistId = artist.ArtistId, TrackId = tag.TrackId};
            return q;
        }

        public static IEnumerable<RelationArtistTrack> GetRelationsArtistTrack(List<FileWithTags> allTags, List<ArtistWithRoles> allArtists)
        {
            ArtistNameExporter artistNameExporter = new ArtistNameExporter();

            return GetRelationsArtistTrack(allTags.Where(t => !string.IsNullOrEmpty(t.Artists)), allArtists,
                    artistNameExporter)
                .Union(
                    GetRelationsAlbumArtistTrack(allTags.Where(t => !string.IsNullOrEmpty(t.AlbumArtists)), allArtists,
                        artistNameExporter), new RelationArtistTrackComparer());
        }

        private static IEnumerable<RelationArtistTrack> GetRelationsArtistTrack(IEnumerable<FileWithTags> allTags, IEnumerable<ArtistWithRoles> allArtists,
            ArtistNameExporter artistNameExporter)
        {
            var q = from artist in allArtists
                from tag in allTags
                where artistNameExporter.ArtistDisambiguated(tag.Artists).Select(a => a.Name.Trim().ToLower())
                    .Contains(artist.Name.Trim().ToLower())
                select new RelationArtistTrack {ArtistId = artist.ArtistId, TrackId = tag.TrackId, Year = tag.Year};
            return q;
        }
        private static IEnumerable<RelationArtistTrack> GetRelationsAlbumArtistTrack(IEnumerable<FileWithTags> allTags, IEnumerable<ArtistWithRoles> allArtists,
            ArtistNameExporter artistNameExporter)
        {
            var q = from artist in allArtists
                from tag in allTags
                where artistNameExporter.ArtistDisambiguated(tag.AlbumArtists).Select(a => a.Name.Trim().ToLower())
                    .Contains(artist.Name.Trim().ToLower())
                select new RelationArtistTrack { ArtistId = artist.ArtistId, TrackId = tag.TrackId, Year = tag.Year };
            return q;
        }
    }
}