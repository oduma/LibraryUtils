using System.Configuration;

namespace LIE.Configuration
{
    public class ExtensionElement : ConfigurationElement
    {
        [ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Key
        {
            get => (string)this["key"];
            set => this["key"] = value;
        }

        [ConfigurationProperty("value", DefaultValue = "", IsRequired = true)]
        public string Value
        {
            get => (string)this["value"];
            set => this["value"] = value;
        }

    }
}
