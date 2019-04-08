using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIE.KnowledgeBaseTypes
{
    public class TransformsLoaded
    {
        public Dictionary<string, string> ArtistNamesMutation = new Dictionary<string, string>
            {
                {"ac; dc", "ac/dc"},
                {"ac;dc", "ac/dc"},
                {"dido armstrong", "dido"},
                {"elo", "electric light orchestra" },
                {"lorenzo \"jovanotti\" cherubini", "jovanotti"},
                {"林默", "missmo"},
                {"mихайло xай", "mihaylo hai"},
                {"桜庭統", "motoi sakuraba"},
                {"Ω▽", "ohmslice" },
                {"ω▽","ohmslice" },
                {"ω▽(ohmslice)","ohmslice" },
                {"rollo", "rollo armstrong"},
                {"つじあやの", "tsuji ayano"},
            };

        public Dictionary<string, string> LatinAlphabetTransformations = new Dictionary<string, string>
            {
                {"á","a" },
                {"à","a" },
                {"ä", "a" },
                {"Å","a" },
                {"â","a" },
                {"ã","a" },
                {"å","a" },
                {"æ","a" },
                {"ß","s" },
                {"č","c" },
                {"ć", "c" },
                {"Ç", "c" },
                {"é", "e" },
                {"è","e" },
                {"ë", "e" },
                {"Ê", "e" },
                {"ė","e" },
                {"ę","e" },
                {"ğ", "g" },
                {"í","i" },
                {"ï","i" },
                {"Î","i" },
                {"ñ","n" },
                {"ń","n" },
                {"ó","o" },
                {"ò","o" },
                {"Ö","o" },
                {"ø","o" },
                {"ô","o" },
                {"ō","o" },
                {"ř","r" },
                {"š","s" },
                { "ș","s"},
                {"$","s" },
                {"ü","u" },
                {"ú","u" },
                {"ū","u" },
                {"ý","y" },
                {"Λ","&" },
                {"�","i" },
            };

    }
}
