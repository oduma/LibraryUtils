using System.Configuration;

namespace Sciendo.Playlist.Translator.Configuration
{
    public class FindAndReplaceConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("fromToParams")]
        public FromToParamsElementCollection FromToParams
        {
            get { return (FromToParamsElementCollection)this["fromToParams"]; }
        }

    }
}
