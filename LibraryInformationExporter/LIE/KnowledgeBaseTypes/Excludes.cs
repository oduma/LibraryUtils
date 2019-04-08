using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIE.KnowledgeBaseTypes
{
    public class Excludes
    {
        public string[] NonTitledInformationFromTitle { get; set; }
        public string[] FeaturedMarkers { get; set; }

        public string PlaceholderAlbumArtists { get; set; }

        public char[] CharactersSeparatorsForWords { get; set; }

        public string[] WordsSeparatorsGlobal { get; set; }

        public string[] ArtistsForSplitting { get; set; }

        public string[] BandsForSplitting { get; set; }
    }
}
