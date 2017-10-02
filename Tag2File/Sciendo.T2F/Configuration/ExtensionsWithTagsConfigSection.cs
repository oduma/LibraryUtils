using System.Configuration;

namespace Sciendo.T2F.Configuration
{
    public class ExtensionsWithTagsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("extensions")]
        public ExtensionElementCollection Extensions
        {
            get { return (ExtensionElementCollection) this["extensions"]; }
        }

    }

    [ConfigurationCollection(typeof(ExtensionElement))]
    public class ExtensionElementCollection : ConfigurationElementCollection
    {
        public ExtensionElement this[int index]
        {
            get { return (ExtensionElement) BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ExtensionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExtensionElement) element).Key;
        }
    }

    public class ExtensionElement : ConfigurationElement
    {
        public ExtensionElement()
        {
        }

        [ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string) this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value", DefaultValue = "", IsRequired = true)]
        public string Value
        {
            get { return (string) this["value"]; }
            set { this["value"] = value; }
        }

    }
}

