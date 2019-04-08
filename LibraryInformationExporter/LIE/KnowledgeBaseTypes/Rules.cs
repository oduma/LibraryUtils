using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIE.KnowledgeBaseTypes
{
    public class Rules
    {
        //99% chance for being a band for artist that start with "The ", "El ", "My " or "New "
        public string[] BandStartWords { get; set; }

        //High chance for artist contain some words to be a band 
        public string[] BandWords { get; set; }

        public int MaxWordsPerArtist { get; set; }
    }
}
