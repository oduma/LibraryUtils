using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIE.KnowledgeBaseTypes
{
    public class RulesLoaded
    {
        //99% chance for being a band for artist that start with "The ", "El ", "My " or "New "
        public string[] BandStartWords = new[]
            { "el", "my", "new", "the",  };

        //High chance for artist contain some words to be a band 
        public string[] BandWords = new[]
        {
                "a",
                "alliance",
                "all",
                "an",
                "association",
                "at",
                "band",
                "banda",
                "boys",
                "bros",
                "brothers",
                "chamber",
                "choir",
                "chorale",
                "city",
                "club",
                "collective",
                "committee",
                "der",
                "duo",
                "ensemble",
                "et",
                "etc.",
                "experience",
                "family",
                "for",
                "foundation",
                "friends",
                "gang",
                "girls",
                "grand",
                "group",
                "grupo",
                "in",
                "kids",
                "kolektiv",
                "las",
                "les",
                "los",
                "men",
                "n'",
                "of",
                "on",
                "or",
                "orchestra",
                "orchestre",
                "orkestar",
                "philarmonic",
                "quartet",
                "quintet",
                "quintetto",
                "royal",
                "sisters",
                "society",
                "squad",
                "symphony",
                "to",
                "trio",
                "twins",
                "und",
                "we",
                "y",

            };

        public int MaxWordsPerArtist = 4;
    }
}
