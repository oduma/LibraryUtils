using System.Configuration;

namespace LIE.Configuration
{
    public class LibraryImporterConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("dataFiles")]
        public DataFileElement DataFiles => (DataFileElement)this["dataFiles"];

        [ConfigurationProperty("extensions")]
        public ExtensionElementCollection Extensions => (ExtensionElementCollection)this["extensions"];

        [ConfigurationProperty("libraryRootFolder", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string LibraryRootFolder
        {
            get => (string)this["libraryRootFolder"];
            set => this["libraryRootFolder"] = value;
        }

        [ConfigurationProperty("reportFrequency", DefaultValue = 1000, IsKey = false, IsRequired = true)]
        public int ReportFrequency
        {
            get => (int)this["reportFrequency"];
            set => this["reportFrequency"] = value;
        }

    }

    public class DataFileElement : ConfigurationElement
    {
        [ConfigurationProperty("notProcessed")]
        public NotProcessedElement NotProcessed => (NotProcessedElement)this["notProcessed"];

        [ConfigurationProperty("facts")]
        public FactsElement Facts => (FactsElement)this["facts"];

        [ConfigurationProperty("relations")]
        public RelationsElement Relations => (RelationsElement)this["relations"];

        [ConfigurationProperty("temporary")]
        public TemporaryElement Temporary => (TemporaryElement)this["temporary"];
    }

    public class NotProcessedElement : ConfigurationElement
    {
        [ConfigurationProperty("allTagsFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string AllTagsFile
        {
            get => (string)this["allTagsFile"];
            set => this["allTagsFile"] = value;
        }
        [ConfigurationProperty("errorsFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string ErrorsFile
        {
            get => (string)this["errorsFile"];
            set => this["errorsFile"] = value;
        }
    }
    public class FactsElement : ConfigurationElement
    {
        [ConfigurationProperty("allArtistsFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string AllArtistsFile
        {
            get => (string)this["allArtistsFile"];
            set => this["allArtistsFile"] = value;
        }

        [ConfigurationProperty("allAlbumsFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string AllAlbumsFile
        {
            get => (string)this["allAlbumsFile"];
            set => this["allAlbumsFile"] = value;
        }

        [ConfigurationProperty("allTracksFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string AllTracksFile
        {
            get => (string)this["allTracksFile"];
            set => this["allTracksFile"] = value;
        }

    }

    public class RelationsElement : ConfigurationElement
    {
        [ConfigurationProperty("artistTrackFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string ArtistTrackFile
        {
            get => (string)this["artistTrackFile"];
            set => this["artistTrackFile"] = value;
        }

        [ConfigurationProperty("albumTrackFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string AlbumTrackFile
        {
            get => (string)this["albumTrackFile"];
            set => this["albumTrackFile"] = value;
        }

        [ConfigurationProperty("composerTrackFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string ComposerTrackFile
        {
            get => (string)this["composerTrackFile"];
            set => this["composerTrackFile"] = value;
        }

        [ConfigurationProperty("featuredArtistTrackFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string FeaturedArtistTrackFile
        {
            get => (string)this["featuredArtistTrackFile"];
            set => this["featuredArtistTrackFile"] = value;
        }

    }

    public class TemporaryElement : ConfigurationElement
    {
        [ConfigurationProperty("allFeaturedFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string AllFeaturedFile
        {
            get => (string)this["allFeaturedFile"];
            set => this["allFeaturedFile"] = value;
        }

        [ConfigurationProperty("featuredArtistsNotFoundFile", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string FeaturedArtistsNotFoundFile
        {
            get => (string)this["featuredArtistsNotFoundFile"];
            set => this["featuredArtistsNotFoundFile"] = value;
        }
    }

}
