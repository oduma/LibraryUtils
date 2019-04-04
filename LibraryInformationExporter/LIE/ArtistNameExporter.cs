using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LIE.DataTypes;

namespace LIE
{
    public class ArtistNameExporter
    {

        public event EventHandler<ArtistNameExporterProgressEventArgs> Progress;

        public ArtistNameExporter()
        {
        }

        public void AddComposers(string composersString, TrackWithArtists trackWithArtists)
        {
            var composers = DisambiguateArtists(composersString, true, false).ToList();
            var composersCount = composers.Count;
            if (composersCount > 0)
            {
                Progress?.Invoke(this, new ArtistNameExporterProgressEventArgs{ComposersFound=composersCount});
                trackWithArtists.Artists.AddRange(composers);
            }
        }

        private IEnumerable<Artist> DisambiguateArtists(string joinedArtists, bool isComposer, 
            bool isFeaturedArtist)
        {
            foreach (var artist in DisambiguateArtist(joinedArtists))
            {
                if (!string.IsNullOrEmpty(artist.Name))
                {
                    artist.IsFeatured = isFeaturedArtist;
                    artist.IsComposer = isComposer;
                    yield return artist;
                }
            }
        }

        private ArtistType DetermineArtistType(List<string> decomposedArtistName)
        {

            //most of the people names start and end with a letter
            if (!char.IsLetter(decomposedArtistName[0][0]) || char.IsDigit(decomposedArtistName.Last()[0]))
                return ArtistType.Band;

            if (decomposedArtistName.Count >= KnowledgeBase.Rules.MaxWordsPerArtist)
                return ArtistType.Band;
            if (KnowledgeBase.Rules.BandStartWords.Any(w =>w == decomposedArtistName[0]))
                return ArtistType.Band;

            if (decomposedArtistName.Any(w => KnowledgeBase.Rules.BandWords.Contains(w)))
                return ArtistType.Band;
            return ArtistType.Artist;
        }

        private IEnumerable<Artist> DisambiguateArtist(string joinedArtists)
        {
            if (!string.IsNullOrEmpty(joinedArtists))
            {
                var simpleLatinLowerCaseJoinedArtists = GetSimpleLatinLowerCaseString(joinedArtists);

            //return pre-canned artists
            foreach (var artistExcludedFromSplitting in KnowledgeBase.Excludes.ArtistsForSplitting)
            {
                var artistExcludedFromSplittingLowerTrimmed = artistExcludedFromSplitting.ToLower().Trim();
                if (simpleLatinLowerCaseJoinedArtists.Contains(artistExcludedFromSplittingLowerTrimmed))
                {
                    simpleLatinLowerCaseJoinedArtists =
                        simpleLatinLowerCaseJoinedArtists.Replace(artistExcludedFromSplittingLowerTrimmed,
                            string.Empty);
                    yield return new Artist
                        {Name = artistExcludedFromSplittingLowerTrimmed, Type = ArtistType.Artist};
                }
            }

            //return pre-canned bands
            if (!string.IsNullOrEmpty(simpleLatinLowerCaseJoinedArtists))
            {
                foreach (var bandExcludedFromSplitting in KnowledgeBase.Excludes.BandsForSplitting)
                {
                    var bandExcludedFromSplittingLowerTrimmed = bandExcludedFromSplitting.ToLower().Trim();
                    if (simpleLatinLowerCaseJoinedArtists.Contains(bandExcludedFromSplittingLowerTrimmed))
                    {
                        simpleLatinLowerCaseJoinedArtists =
                            simpleLatinLowerCaseJoinedArtists.Replace(bandExcludedFromSplittingLowerTrimmed,
                                string.Empty);
                        yield return new Artist
                            {Name = KnowledgeBase.Transforms.ArtistNamesMutation.Keys.Contains(bandExcludedFromSplittingLowerTrimmed)
                                ? KnowledgeBase.Transforms.ArtistNamesMutation[bandExcludedFromSplittingLowerTrimmed]
                                : bandExcludedFromSplittingLowerTrimmed, Type = ArtistType.Band};
                    }
                }
            }

            if (!string.IsNullOrEmpty(simpleLatinLowerCaseJoinedArtists))
            {
                var firstPassSplitParts = simpleLatinLowerCaseJoinedArtists.Split(
                    KnowledgeBase.Excludes.CharactersSeparatorsForWords, StringSplitOptions.RemoveEmptyEntries);
                foreach (var firstPassSplitPart in firstPassSplitParts)
                {
                    var wordParts = firstPassSplitPart.Split(new char[] {KnowledgeBase.Spliters.WordsSimpleSplitter},
                        StringSplitOptions.RemoveEmptyEntries);
                    var decomposedArtistName= new List<string>();
                    Artist artist;
                    foreach (var wordPart in wordParts)
                    {
                        if (!KnowledgeBase.Excludes.WordsSeparatorsGlobal.Contains(wordPart))
                        {
                            decomposedArtistName.Add(wordPart);
                        }
                        else
                        {
                            artist = ComposeArtistAndType(ref decomposedArtistName);
                            if (artist != null) 
                                yield return artist;
                        }
                    }

                    artist = ComposeArtistAndType(ref decomposedArtistName);
                    if (artist != null)
                        yield return artist;
                }
            }
        }
    }

        private Artist ComposeArtistAndType(ref List<string> decomposedArtistName)
        {
            if (decomposedArtistName.Count > 0)
            {

                var artist = new Artist
                {
                    Name = ComposeArtistName(decomposedArtistName),
                    Type = DetermineArtistType(decomposedArtistName)
                };
                decomposedArtistName = new List<string>();
                return artist;
            }

            return null;
        }

        private string ComposeArtistName(List<string> decomposedArtistName)
        {
            var recomposedArtistName =
                string.Join(KnowledgeBase.Spliters.WordsSimpleSplitter.ToString(), decomposedArtistName);

            return KnowledgeBase.Transforms.ArtistNamesMutation.Keys.Contains(recomposedArtistName)
                ? KnowledgeBase.Transforms.ArtistNamesMutation[recomposedArtistName]
                : recomposedArtistName;
        }

        private static string GetSimpleLatinLowerCaseString(string input)
        {
            var result = input.ToLower().Trim();
            foreach (var key in KnowledgeBase.Transforms.LatinAlphabetTransformations.Keys)
            {
                result = result.Replace(key.ToLower(), KnowledgeBase.Transforms.LatinAlphabetTransformations[key]);
            }

            return result;
        }

        public void AddArtists(string artistsString, TrackWithArtists trackWithArtistsWithRoles)
        {
            var artists = DisambiguateArtists(artistsString, false, false).ToList();
            var artistsCount = artists.Count();
            if (artistsCount > 0)
            {
                Progress?.Invoke(this, new ArtistNameExporterProgressEventArgs{ArtistsFound = artistsCount});
                trackWithArtistsWithRoles.Artists.AddRange(artists);
            }
        }

        public void AddAlbumArtists(string albumArtistsString,
            TrackWithArtists trackWithArtistsWithRoles)
        {
            var albumArtists = DisambiguateArtists(albumArtistsString, false, false).Where(a =>
                a.Name.ToLower().Trim() != KnowledgeBase.Excludes.PlaceholderAlbumArtists.ToLower().Trim()).ToList();
            if (albumArtists.Count > 0)
            {
                Progress?.Invoke(this,new ArtistNameExporterProgressEventArgs{AlbumArtistsFound = albumArtists.Count});
                trackWithArtistsWithRoles.Artists.AddRange(albumArtists);
            }
        }

        public void AddFeaturedArtists(string title, TrackWithArtists trackWithArtistsWithRoles)
        {
            List<Artist> featuredArtists= new List<Artist>();
            var possibleArtistsFeatures = Regex.Matches(title, KnowledgeBase.Spliters.FeaturedArtistsInTheTitle);
            if (possibleArtistsFeatures.Count > 0)
            {
                foreach (var possibleArtistsFeature in possibleArtistsFeatures)
                {
                    var possibleArtistFeatureWithoutMarkers = possibleArtistsFeature.ToString();
                    foreach (var marker in KnowledgeBase.Excludes.FeaturedMarkers)
                    {
                        possibleArtistFeatureWithoutMarkers = possibleArtistFeatureWithoutMarkers.Replace(marker, string.Empty);
                    }
                    featuredArtists.AddRange(DisambiguateArtists(possibleArtistFeatureWithoutMarkers, false,true));
                }
            }

            if (featuredArtists.Count > 0)
            {
                Progress?.Invoke(this, new ArtistNameExporterProgressEventArgs{FeaturedArtistsFound = featuredArtists.Count});
                trackWithArtistsWithRoles.Artists.AddRange(featuredArtists);
            }
        }

        public List<Artist> AggregateArtists(List<ArtistWithTracks> allArtists)
        {
            return allArtists.Select(a => new Artist
            {
                ArtistId = a.ArtistId,
                Name = a.Name,
                Type = a.Tracks.First().Type,

            }).ToList();
        }
    }
}
