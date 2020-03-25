using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LIE.DataTypes;
using LIE.KnowledgeBaseTypes;

namespace LIE
{
    public class ArtistNameExporter
    {
        private readonly KnowledgeBase _knowledgeBase;

        public ArtistNameExporter(KnowledgeBase knowledgeBase)
        {
            _knowledgeBase = knowledgeBase;
        }
        public event EventHandler<ArtistNameExporterProgressEventArgs> Progress;

        public void AddComposers(string composersString, TrackWithArtists trackWithArtists)
        {
            var composers = DisambiguateArtists(composersString, true, false).ToList();
            composers.ForEach(c=>c.IsComposer=true);
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

            if (decomposedArtistName.Count >= _knowledgeBase.Rules.MaxWordsPerArtist)
                return ArtistType.Band;
            if (_knowledgeBase.Rules.BandStartWords.Any(w =>w == decomposedArtistName[0]))
                return ArtistType.Band;

            if (decomposedArtistName.Any(w => _knowledgeBase.Rules.BandWords.Contains(w)))
                return ArtistType.Band;
            return ArtistType.Artist;
        }

        private IEnumerable<Artist> DisambiguateArtist(string joinedArtists)
        {
            if (!string.IsNullOrEmpty(joinedArtists))
            {
                var simpleLatinLowerCaseJoinedArtists = GetSimpleLatinLowerCaseString(joinedArtists);

            //return pre-canned artists
            foreach (var artistExcludedFromSplitting in _knowledgeBase.Excludes.ArtistsForSplitting)
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
                foreach (var bandExcludedFromSplitting in _knowledgeBase.Excludes.BandsForSplitting)
                {
                    var bandExcludedFromSplittingLowerTrimmed = bandExcludedFromSplitting.ToLower().Trim();
                    if (simpleLatinLowerCaseJoinedArtists.Contains(bandExcludedFromSplittingLowerTrimmed))
                    {
                        simpleLatinLowerCaseJoinedArtists =
                            simpleLatinLowerCaseJoinedArtists.Replace(bandExcludedFromSplittingLowerTrimmed,
                                string.Empty);
                        yield return new Artist
                            {Name = _knowledgeBase.Transforms.ArtistNamesMutation.Keys.Contains(bandExcludedFromSplittingLowerTrimmed)
                                ? _knowledgeBase.Transforms.ArtistNamesMutation[bandExcludedFromSplittingLowerTrimmed]
                                : bandExcludedFromSplittingLowerTrimmed, Type = ArtistType.Band};
                    }
                }
            }

            if (!string.IsNullOrEmpty(simpleLatinLowerCaseJoinedArtists))
            {
                var firstPassSplitParts = simpleLatinLowerCaseJoinedArtists.Split(
                    _knowledgeBase.Excludes.CharactersSeparatorsForWords, StringSplitOptions.RemoveEmptyEntries);
                foreach (var firstPassSplitPart in firstPassSplitParts)
                {
                    var wordParts = firstPassSplitPart.Split(new[] {_knowledgeBase.Spliters.WordsSimpleSplitter},
                        StringSplitOptions.RemoveEmptyEntries);
                    var decomposedArtistName= new List<string>();
                    Artist artist;
                    foreach (var wordPart in wordParts)
                    {
                        if (!_knowledgeBase.Excludes.WordsSeparatorsGlobal.Contains(wordPart))
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
                string.Join(_knowledgeBase.Spliters.WordsSimpleSplitter.ToString(), decomposedArtistName);

            return _knowledgeBase.Transforms.ArtistNamesMutation.Keys.Contains(recomposedArtistName)
                ? _knowledgeBase.Transforms.ArtistNamesMutation[recomposedArtistName]
                : recomposedArtistName.Trim();
        }

        private string GetSimpleLatinLowerCaseString(string input)
        {
            var result = input.ToLower().Trim();
            foreach (var key in _knowledgeBase.Transforms.LatinAlphabetTransformations.Keys)
            {
                result = result.Replace(key.ToLower(), _knowledgeBase.Transforms.LatinAlphabetTransformations[key]);
            }

            return result;
        }

        public void AddArtists(string artistsString, TrackWithArtists trackWithArtistsWithRoles)
        {
            var artists = DisambiguateArtists(artistsString, false, false).ToList();
            var artistsCount = artists.Count;
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
                a.Name.ToLower().Trim() != _knowledgeBase.Excludes.PlaceholderAlbumArtists.ToLower().Trim()).ToList();
            if (albumArtists.Count > 0)
            {
                Progress?.Invoke(this,new ArtistNameExporterProgressEventArgs{AlbumArtistsFound = albumArtists.Count});
                trackWithArtistsWithRoles.Artists.AddRange(albumArtists);
            }
        }

        public void AddFeaturedArtists(string title, TrackWithArtists trackWithArtistsWithRoles)
        {
            if (!string.IsNullOrEmpty(title))
            {
                title = GetSimpleLatinLowerCaseString(title).ReplaceAll(_knowledgeBase.Excludes.NonTitledInformationFromTitle,string.Empty);
                List<Artist> featuredArtists = new List<Artist>();
                var possibleArtistsFeatures = Regex.Matches(title, _knowledgeBase.Spliters.FeaturedArtistsInTheTitle);
                if (possibleArtistsFeatures.Count > 0)
                {
                    foreach (var possibleArtistsFeature in possibleArtistsFeatures)
                    {
                        var possibleArtistFeatureWithoutMarkers = possibleArtistsFeature.ToString();
                        foreach (var marker in _knowledgeBase.Excludes.FeaturedMarkers)
                        {
                            possibleArtistFeatureWithoutMarkers =
                                possibleArtistFeatureWithoutMarkers.Replace(marker, string.Empty);
                        }

                        featuredArtists.AddRange(DisambiguateArtists(possibleArtistFeatureWithoutMarkers, false, true));
                    }
                }
                featuredArtists.ForEach(a=>a.IsFeatured=true);
                if (featuredArtists.Count > 0)
                {
                    Progress?.Invoke(this,
                        new ArtistNameExporterProgressEventArgs {FeaturedArtistsFound = featuredArtists.Count});
                    trackWithArtistsWithRoles.Artists.AddRange(featuredArtists);
                }
            }
        }

        public List<Artist> AggregateArtists(List<ArtistWithTracks> allArtists)
        {
            return allArtists.Select(a => new Artist
            {
                ArtistId = a.ArtistId,
                Name = a.Name,
                Type = a.Tracks.First().Type,
                IsComposer = a.Tracks.Any(t=>t.IsComposer),
                IsFeatured=a.Tracks.Any(t=>t.IsFeatured)

            }).OrderBy(a=>a.Name).ToList();
        }
    }
}
