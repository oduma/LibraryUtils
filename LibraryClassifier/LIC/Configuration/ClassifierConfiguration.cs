using Sciendo.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIC.Configuration
{
    public class ClassifierConfiguration
    {
        [ConfigProperty("NotProcessedFile")]
        public string NotProcessedFile { get; set; }

        [ConfigProperty("Facts")]
        public Facts Facts { get; set; }

        [ConfigProperty("Relations")]
        public Relations Relations { get; set; }

        [ConfigProperty("KnowledgeBase")]
        public string KnowledgeBase { get; set; }
    }

    public class Facts
    {
        public string AllArtistsFile{get;set;}

        public string AllAlbumsFile{ get; set; }

        public string AllTracksFile{ get; set; }

    }

    public class Relations
    {
        public string ArtistTrackFile { get; set; }

        public string AlbumTrackFile { get; set; }

        public string ComposerTrackFile { get; set; }

        public string FeaturedArtistTrackFile { get; set; }

    }


}
