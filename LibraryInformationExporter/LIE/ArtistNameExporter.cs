using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;

namespace LIE
{
    public class ArtistNameExporter
    {
        private readonly IStorage _storage;
        private readonly List<ArtistWithRole> _previousArtistNames;
        private readonly ArtistNameMutations _artistNameMutations;

        public event EventHandler<TagPartReadEventArgs> ArtistRead; 

        public ArtistNameExporter(IStorage storage, List<ArtistWithRole> previousArtistNames):this()
        {
            _storage = storage;
            _previousArtistNames = previousArtistNames;
        }

        public ArtistNameExporter()
        {
            _artistNameMutations = new ArtistNameMutations();
        }

        public List<ArtistWithRole> GetFullListOfArtistNames(string path, string[] extensions = null)
        {
            
            List<ArtistWithRole> artistsWithRoles = new List<ArtistWithRole>();
            foreach (var fileName in _storage.Directory.GetFiles(path, SearchOption.AllDirectories, extensions))
            {
                TagLib.Tag tag;
                try
                {
                    tag = _storage.File.ReadTag(fileName).Tag;
                }
                catch
                {
                    tag = null;
                }

                if (tag != null)
                {

                    if (!string.IsNullOrEmpty(tag.JoinedAlbumArtists))
                        artistsWithRoles.AddRange(ArtistDisambiguated(tag.JoinedAlbumArtists));

                    if (!string.IsNullOrEmpty(tag.JoinedPerformers))
                        artistsWithRoles.AddRange(ArtistDisambiguated(tag.JoinedPerformers));

                    if (tag.Composers != null)
                    {
                        artistsWithRoles.AddRange(ComposersDisambiguated(tag.Composers));
                    }
                }
            }

            return artistsWithRoles;
        }

        internal List<ArtistWithRole> ComposersDisambiguated(string[] composers)
        {
            List<ArtistWithRole> artistsWithRoles = new List<ArtistWithRole>();
            foreach (var composer in composers)
            {
                var composerSplits = composer.Split(new char[] { '/' });
                foreach (var composerSplit in composerSplits)
                    artistsWithRoles.AddRange(ArtistDisambiguated(composerSplit,ArtistRole.Composer));
            }

            return artistsWithRoles;
        }
        private IEnumerable<ArtistWithRole> ArtistDisambiguated(string joinedArtists, ArtistRole artistRole=ArtistRole.Artist)
        {
            foreach (var artistName in DisambiguateArtistName(joinedArtists))
            {
                ArtistRead?.Invoke(this, new TagPartReadEventArgs(artistName));
                yield return new ArtistWithRole { Name = artistName, Role = artistRole };
            }
        }

        private IEnumerable<string> DisambiguateArtistName(string artistName)
        {
            var artistNameParts = new[] {""};
            var exceptionRules = new[] {"AC;DC", "AC; DC", "AC/DC", "AC/ DC"};
            artistNameParts = exceptionRules.Contains(artistName)
                ? new[] {"AC/DC"}
                : artistName.Split(new[] {";", "vs.", "Vs.", "feat.", "Feat.", "ft.", "Ft.", " feat "},
                    StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawArtistNamePart in artistNameParts)
            {
                var artistNamePart = rawArtistNamePart.Trim();
                if (_artistNameMutations.FirstLevelArtistNameMutations.Keys.Contains(artistNamePart))
                {
                    foreach (var artistNameMutation in _artistNameMutations.FirstLevelArtistNameMutations[artistNamePart])
                    {
                        if (!string.IsNullOrEmpty(artistNameMutation))
                            yield return $@"""{artistNameMutation.Trim().Replace(@"""", @"""""")}""";
                    }
                }
                else
                {
                    yield return $@"""{artistNamePart.Trim().Replace(@"""", @"""""")}""";
                }
            }
        }

        public bool ArtistPreviouslyLoaded(string artistName)
        {
            return _previousArtistNames.Any(n => n.Name.ToLower() == artistName.ToLower());
        }

        public ArtistWithRole Process(ArtistWithRole artistWithRole)
        {
            if (artistWithRole.Name.Contains(","))
            {
                artistWithRole.ProcessedName = artistWithRole.Name.Replace(", ", "&").Replace(",", "&").Replace("/","&");
                return artistWithRole;
            }

            //artistWithRole.Name = artistWithRole.Name.Replace("&", "and");
            return artistWithRole;
        }

        internal List<ArtistWithRoles> GetFullListOfArtistNamesFromTags(List<FileWithTag> allTags)
        {
            IEnumerable<ArtistWithRole> artistsWithRole = allTags
                .Where(t => t.TagType == TagType.AlbumArtist || t.TagType == TagType.Artist ||
                            t.TagType == TagType.Composers).SelectMany(t =>
                    (t.TagType == TagType.Composers)
                        ? ComposersDisambiguated(t.TagContents.Split(new char[] {';'}))
                        : ArtistDisambiguated(t.TagContents));

            List<ArtistWithRoles> artistsWithRoles = new List<ArtistWithRoles>();
            var uniqueArtistsWithRole = artistsWithRole.Distinct(new ArtistWithRoleEqualityComparer());
            foreach (var uniqueArtistWithRole in uniqueArtistsWithRole)
            {
                var artistWithRoles = artistsWithRoles.FirstOrDefault(a =>
                    a.Name.ToLower() == uniqueArtistWithRole.Name.ToLower() ||
                    ((!string.IsNullOrEmpty(a.ProcessedName) && !string.IsNullOrEmpty(uniqueArtistWithRole.ProcessedName))) 
                    && a.ProcessedName.ToLower() == uniqueArtistWithRole.ProcessedName.ToLower());
                if (artistWithRoles == null)
                    artistsWithRoles.Add(new ArtistWithRoles
                    {
                        ArtistId = uniqueArtistWithRole.ArtistId, Name = uniqueArtistWithRole.Name,
                        ProcessedName = uniqueArtistWithRole.ProcessedName,
                        ArtistRoles = (uniqueArtistWithRole.Role == ArtistRole.Composer)
                            ? new List<ArtistRole> {uniqueArtistWithRole.Role, ArtistRole.Artist}
                            : new List<ArtistRole> {uniqueArtistWithRole.Role}
                    });
                else
                {
                    artistWithRoles.ArtistRoles.Add(uniqueArtistWithRole.Role);
                }
            }
            return artistsWithRoles.OrderBy(a => a.Name).Select(a=>AddBandQualifier(a)).ToList();
        }
        private ArtistWithRoles AddBandQualifier(ArtistWithRoles artistWithRoles)
        {
            if (artistWithRoles == null || string.IsNullOrEmpty(artistWithRoles.Name))
                return null;
            
            //99% chance for being a band for artist that start with "The ", "El ", "My " or "New "
            string[] bandStartWords = new[] {"the", "el", "new", "my"};
            if (bandStartWords.Any(w => w == artistWithRoles.Name.Split(new[] {' '})[0]) &&
                artistWithRoles.ArtistRoles.All(r => r != ArtistRole.Band))
                artistWithRoles.ArtistRoles.Add(ArtistRole.Band);
            //High chance for artist contain some words to be a band 
            string[] bandWords = new[]
            {
                "and", "&", "band", "orchestra", "orkestar", "symphony", "philarmonic", "chamber", "royal", "at", "for",
                "choir", "a", "y", "aa","city","of", "in","quartet", "in", "on","all","an","trio", "n'","foundation","etc.",
                "av", "brothers", "family", "boys", "bros", "gang", "by","society", "club","quintet","twins","sisters",
                "kids","experience", "chorale", "banda", "or", "to", "alliance", "und", "der", "duo", "die","association",
                "kolektiv", "grupo", "et", "friends", "committee", "grand", "we", "las", "los", "collective", "les",
                "group", "men", "orchestre", "girls", "ensemble","quintetto", "squad",
            }; 
            if ((artistWithRoles.Name.ToLower().Split(new []{' '}).Any(w=>bandWords.Contains(w))) &&
                artistWithRoles.ArtistRoles.All(r => r != ArtistRole.Band))
            {
                artistWithRoles.ArtistRoles.Add(ArtistRole.Band);
            }
            //most of people have only 4 names
            if (artistWithRoles.Name.Split(new[] {' '}).Length >= 4 &&
                artistWithRoles.ArtistRoles.All(r => r != ArtistRole.Band))
            {
                artistWithRoles.ArtistRoles.Add(ArtistRole.Band);
            }
            //most of the people names start and end with a letter
            if ((!char.IsLetter(artistWithRoles.Name[0]) ||
                 char.IsDigit((artistWithRoles.Name[artistWithRoles.Name.Length - 1]))) &&
                artistWithRoles.ArtistRoles.All(r => r != ArtistRole.Band))
            {
                artistWithRoles.ArtistRoles.Add(ArtistRole.Band);
            }

            string[] exceptionRules = new[] {"2pac", "antonio carles marques pinto", "aram mp3", "B L A C K I E", "Alvin Pleasant Delaney Carter",
                "anna of the north","Boydston John D. III","Jennifer Evans van der Harten", "John 5", "KH of Moscrill",
                @"Luther ""Guitar Junior"" Johnson", "Maria Isabel Garcia Asensio","Martin Luther King Jr.", "Michael L. Williams II",
                "Michiel Van Der Kuy","Niels Henning Orsted Pedersen", "The Reverend Peyton",
            };
            if (exceptionRules.Contains(artistWithRoles.Name.ToLower()))
            {
                var band = artistWithRoles.ArtistRoles.FirstOrDefault(r => r == ArtistRole.Band);
                if (band == ArtistRole.Band)
                    artistWithRoles.ArtistRoles.Remove(band);

            }

            return artistWithRoles;
        }
    }
}
