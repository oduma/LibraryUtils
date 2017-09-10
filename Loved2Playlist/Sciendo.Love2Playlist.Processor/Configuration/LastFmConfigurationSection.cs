using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Love2Playlist.Processor.Configuration
{
    public class LastFmConfigurationSection:ConfigurationSection
    {
        [ConfigurationProperty("root", DefaultValue = "http://localhost:7474/db/dat", IsRequired = true)]
        public string Root
        {
            get { return (string)this["root"]; }
            set { this["root"] = value; }
        }

        [ConfigurationProperty("appKey", DefaultValue = "http://localhost:7474/db/dat", IsRequired = true)]
        public string AppKey
        {
            get { return (string)this["appKey"]; }
            set { this["appKey"] = value; }
        }
        [ConfigurationProperty("user", DefaultValue = "http://localhost:7474/db/dat", IsRequired = true)]
        public string User
        {
            get { return (string)this["user"]; }
            set { this["user"] = value; }
        }

    }
}
