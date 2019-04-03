using System.Collections.Generic;
using System.IO;
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
                        { ArtistId = artistWithTracks.ArtistId, TrackId = track.TrackId };
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
                        { ArtistId = artistWithTracks.ArtistId, TrackId = track.TrackId };
                }
            }
        }
        //public static IEnumerable<RelationArtistTrack> GetRelationsArtistTrack(List<FileWithTags> allTags, List<Artist> allArtists)
        //{
        //    ArtistNameExporter artistNameExporter = new ArtistNameExporter();

        //    return GetRelationsArtistTrack(allTags.Where(t => !string.IsNullOrEmpty(t.Artists)), allArtists,
        //            artistNameExporter)
        //        .Union(
        //            GetRelationsAlbumArtistTrack(allTags.Where(t => !string.IsNullOrEmpty(t.AlbumArtists)), allArtists,
        //                artistNameExporter), new RelationArtistTrackComparer());
        //}

        //private static IEnumerable<RelationArtistTrack> GetRelationsArtistTrack(IEnumerable<FileWithTags> allTags, IEnumerable<Artist> allArtists,
        //    ArtistNameExporter artistNameExporter)
        //{
        //    var q = from artist in allArtists
        //        from tag in allTags
        //        where artistNameExporter.ArtistDisambiguated(tag.Artists).Select(a => a.Name.Trim().ToLower())
        //            .Contains(artist.Name.Trim().ToLower())
        //        select new RelationArtistTrack {ArtistId = artist.ArtistId, TrackId = tag.TrackId, Year = tag.Year};
        //    return q;
        //}
        //private static IEnumerable<RelationArtistTrack> GetRelationsAlbumArtistTrack(IEnumerable<FileWithTags> allTags, IEnumerable<Artist> allArtists,
        //    ArtistNameExporter artistNameExporter)
        //{
        //    var q = from artist in allArtists
        //        from tag in allTags
        //        where artistNameExporter.ArtistDisambiguated(tag.AlbumArtists).Select(a => a.Name.Trim().ToLower())
        //            .Contains(artist.Name.Trim().ToLower())
        //        select new RelationArtistTrack { ArtistId = artist.ArtistId, TrackId = tag.TrackId, Year = tag.Year };
        //    return q;
        //}

        //public static List<RelationArtistTrack> GetRelationsFeaturedExistingArtistTrack(List<FileWithTags> allTags, 
        //    List<TrackWithFile> rawFeatureArtists, List<Artist> allArtists,List<TrackWithFile> partiallyNotFound)
        //{
        //    List<RelationArtistTrack> results = new List<RelationArtistTrack>();
        //    ArtistNameExporter artistNameExporter= new ArtistNameExporter();
        //    foreach (var rawFeatureArtist in rawFeatureArtists)
        //    {
        //        var tag = allTags.FirstOrDefault(t => t.TrackId == rawFeatureArtist.TrackId);
        //        foreach (var rawFeaturedArtistName in SplitArtistsIfPossible(rawFeatureArtist))
        //        {
        //            var artistsName = artistNameExporter.DisambiguateArtistName(rawFeaturedArtistName);
        //            foreach (var artistName in artistsName)
        //            {
        //                var existingArtist =
        //                    allArtists.FirstOrDefault(a => a.Name.ToLower().Trim() == artistName.ToLower().Trim());
        //                if (existingArtist != null)
        //                {
        //                    results.Add(new RelationArtistTrack
        //                    {
        //                        ArtistId = existingArtist.ArtistId,
        //                        TrackId = rawFeatureArtist.TrackId,
        //                        Year = tag.Year
        //                    });
        //                }
        //                else
        //                {
        //                    partiallyNotFound.Add(new TrackWithFile
        //                        {File = rawFeatureArtist.File, Name = artistName, TrackId = rawFeatureArtist.TrackId});
        //                }
        //            }
        //        }
        //    }
        //    return results;
        //}

        //private static string[] SplitArtistsIfPossible(TrackWithFile rawFeatureArtist)
        //{
        //    var doNotSplit = new string[] {
        //    };
        //    if (doNotSplit.Any(d => d.ToLower().Trim() == rawFeatureArtist.Name.ToLower().Trim()))
        //        return new[] {rawFeatureArtist.Name};
        //    return rawFeatureArtist.Name.Split(new[] {',', '&'});
        //}

        //public static List<RelationArtistTrack> GetRelationsFeaturedNewArtistTrack(List<FileWithTags> allTags, 
        //    List<Artist> newArtists, 
        //    List<TrackWithFile> featuredArtistsNotFound)
        //{
        //    List<RelationArtistTrack> results = new List<RelationArtistTrack>();
        //    foreach (var featuredArtistNotFound in featuredArtistsNotFound)
        //    {
        //        results.Add(new RelationArtistTrack
        //        {
        //            ArtistId = newArtists
        //                .First(a => a.Name.Trim().ToLower() == featuredArtistNotFound.Name.Trim().ToLower()).ArtistId,
        //            TrackId = featuredArtistNotFound.TrackId,
        //            Year = allTags.First(t => t.TrackId == featuredArtistNotFound.TrackId).Year
        //        });
        //    }

        //    return results;
        //}
    }
}