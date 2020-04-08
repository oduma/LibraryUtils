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

}
