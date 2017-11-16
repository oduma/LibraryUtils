using System.Configuration;

namespace Sciendo.Playlist.Translator.Configuration
{
    [ConfigurationCollection(typeof(ExtensionElement))]
    public class FromToParamsElementCollection : ConfigurationElementCollection
    {
        public FromToParamsElement this[int index]
        {
            get { return (FromToParamsElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FromToParamsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FromToParamsElement)element).From;
        }
    }
}