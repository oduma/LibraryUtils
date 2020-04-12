using Sciendo.ArtistClassifier.NLP.NER.Configuration;
using Sciendo.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AC2.Configuration
{
    public class ArtistClassifierPass2Configuration
    {
        [ConfigProperty("inputFile")]
        public string AllArtistsInputFile { get; set; }

        [ConfigProperty("outputFile")]
        public string AllArtistsOutputFile { get; set; }

        [ConfigProperty("nlpConfig")]
        public NlpConfig NlpConfig { get; set; }

    }
}
