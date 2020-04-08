using LIC.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIC
{
    public static class RelationsHelper
    {
        public static IEnumerable<RelationComposerTrack> GetRelationsComposerTrack(ArtistWithTracks artistWithTracks)
        {
            foreach (var track in artistWithTracks.Tracks)
            {
                if (track.IsComposer)
                {
                    yield return new RelationComposerTrack
                    { ArtistId = artistWithTracks.ArtistId, TrackId = track.TrackId };
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
                    { ArtistId = artistWithTracks.ArtistId, TrackId = track.TrackId, Year = track.Year };
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
                    { ArtistId = artistWithTracks.ArtistId, TrackId = track.TrackId, Year = track.Year };
                }
            }
        }


    }
}
