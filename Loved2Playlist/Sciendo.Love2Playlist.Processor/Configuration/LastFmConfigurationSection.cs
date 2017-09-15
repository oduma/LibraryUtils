using System;
using System.Configuration;

namespace Sciendo.Love2Playlist.Processor.Configuration
{
    public class LastFmConfigurationSection:ConfigurationSection,IUrlProvider
    {
        [ConfigurationProperty("root", DefaultValue = "http://ws.audioscrobbler.com/2.0/", IsRequired = true)]
        public string Root
        {
            get { return (string)this["root"]; }
            set { this["root"] = value; }
        }

        [ConfigurationProperty("method", IsRequired = true)]
        public string Method
        {
            get { return (string)this["method"]; }
            set { this["method"] = value; }
        }

        [ConfigurationProperty("appKey", DefaultValue = "67b6145c521d4ca0e31ef35c3032d320", IsRequired = true)]
        public string AppKey
        {
            get { return (string)this["appKey"]; }
            set { this["appKey"] = value; }
        }
        [ConfigurationProperty("user", DefaultValue = "scentMaster", IsRequired = true)]
        public string User
        {
            get { return (string)this["user"]; }
            set { this["user"] = value; }
        }

        [ConfigurationProperty("format", DefaultValue = "json", IsRequired = true)]
        public string Format
        {
            get { return (string)this["format"]; }
            set { this["format"] = value; }
        }

        public Uri GetUrl(int pageNumber)
        {
            return new Uri($"{Root}?method={Method}&user={User}&api_key={AppKey}&format={Format}&page={pageNumber}");
        }
    }
}
