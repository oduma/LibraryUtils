using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Translator.Configuration
{
    public class FindAndReplaceConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("fromToParams")]
        public FromToParamsElementCollection FromToParams
        {
            get { return (FromToParamsElementCollection)this["`fromToParams"]; }
        }

    }
}
