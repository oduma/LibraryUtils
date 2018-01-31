using System.Configuration;

namespace Sciendo.Mixx.DataAccess.Configuration
{
    public class MixxxConfigurationSection: ConfigurationSection
    {
        [ConfigurationProperty("mixxxDatabaseFile",
            DefaultValue = @"C:\Users\octo\AppData\Local\Mixxx\mixxxdb.sqlite", IsRequired = true)]
        public string MixxxDatabaseFile
        {
            get { return (string)this["mixxxDatabaseFile"]; }
            set { this["mixxxDatabaseFile"] = value; }
        }

    }
}
