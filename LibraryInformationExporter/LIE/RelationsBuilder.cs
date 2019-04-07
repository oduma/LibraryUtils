using System.Collections.Generic;
using System.Linq;
using LIE.DataTypes;

namespace LIE
{
    internal static class RelationsBuilder
    {
        public static List<RelationTrackAlbum> GetRelationsTrackAlbum(List<AlbumWithLocationAndTracks> allAlbums)
        {
            List<RelationTrackAlbum> result=new List<RelationTrackAlbum>();
            allAlbums.ForEach(a => result.AddRange(a.Tracks.Select(t => new RelationTrackAlbum
                {AlbumId = a.AlbumId, TrackId = t.TrackId, TrackNo = t.TrackNo})));
            return result;
        }

        public static IEnumerable<RelationComposerTrack> GetRelationsComposerTrack(ArtistWithTracks artistWithTracks)
        {
            foreach (var track in artistWithTracks.Tracks)
            {
                if (track.IsComposer)
                {
                    yield return new RelationComposerTrack
                        {ArtistId = artistWithTracks.ArtistId, TrackId = track.TrackId};
                }
            }
        }

        public static IEnumerable<RelationArtistTrack> GetRelationsArtistTrack(ArtistWithTracks artistWithTracks)
        {
            foreach (var track in artistWithTracks.Tracks)
            {
                if (!track.IsComposer && !track.IsFeatured)
                {
                    yield return new RelationArtistTrack
                        { ArtistId = artistWithTracks.ArtistId, TrackId = track.TrackId,Year = track.Year};
                }
            }
        }
        public static IEnumerable<RelationArtistTrack> GetRelationsFeaturedArtistTrack(ArtistWithTracks artistWithTracks)
        {
            foreach (var track in artistWithTracks.Tracks)
            {
                if (track.IsFeatured)
                {
                    yield return new RelationArtistTrack
                        { ArtistId = artistWithTracks.ArtistId, TrackId = track.TrackId, Year = track.Year};
                }
            }
        }
    }
}