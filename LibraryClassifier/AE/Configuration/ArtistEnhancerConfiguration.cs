using Sciendo.ArtistClassifier.NLP.NER.Configuration;
using Sciendo.Config;
using Sciendo.Wiki.Search.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE.Configuration
{
    public class ArtistEnhancerConfiguration
    {
        [ConfigProperty("inputFile")]
        public string AllArtistsInputFile { get; set; }

        [ConfigProperty("outputFile")]
        public string AllArtistsOutputFile { get; set; }

        [ConfigProperty("bandsWithoutMembersFile")]
        public string BandsWithoutMembersFile { get; set; }

        [ConfigProperty("relationshipBandArtistFile")]
        public string RelationshipBandArtistFile { get; set; }

        [ConfigProperty("wikiSearch")]
        public WikiSearchConfig WikiSearchConfig { get; set; }

        [ConfigProperty("nlpConfig")]
        public NlpConfig NlpConfig { get; set; }

        [ConfigProperty("knowledgeBaseFile")]
        public string KnowledgeBaseFile { get; set; }


    }
}
