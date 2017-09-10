using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Love2Playlist.Processor.Configuration
{
    public class PlaylistConfigurationSection: ConfigurationSection
    {
        [ConfigurationProperty("fileName", DefaultValue = "lastFmLoved", IsRequired = true)]
        public string FileName
        {
            get { return (string)this["fileName"]; }
            set { this["fileName"] = value; }
        }

        [ConfigurationProperty("clementineDatabaseFile",
            DefaultValue = @"C:\Users\octo\.config\Clementine\clementine.db", IsRequired = true)]
        public string ClementineDatabaseFile
        {
            get { return (string)this["clementineDatabaseFile"]; }
            set { this["clementineDatabaseFile"] = value; }
        }

    }
}
