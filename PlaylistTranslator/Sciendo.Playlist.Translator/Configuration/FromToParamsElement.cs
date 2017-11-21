using System.Configuration;

namespace Sciendo.Playlist.Translator.Configuration
{
    public class FromToParamsElement : ConfigurationElement
    {
        [ConfigurationProperty("from", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string From
        {
            get { return (string)this["from"]; }
            set { this["from"] = value; }
        }

        [ConfigurationProperty("to", DefaultValue = "", IsRequired = true)]
        public string To
        {
            get { return (string)this["to"]; }
            set { this["to"] = value; }
        }

        [ConfigurationProperty("priority", DefaultValue = 0, IsRequired = true)]
        public int Priority
        {
            get { return (int)this["priority"]; }
            set { this["priority"] = value; }
        }

    }
}