using System.Configuration;

namespace Sciendo.T2F.Configuration
{
    public class RuleElement: ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = RuleType.None, IsKey = true, IsRequired = true)]
        public RuleType Type
        {
            get { return (RuleType)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("pattern", DefaultValue = "", IsRequired = true)]
        public string Pattern
        {
            get { return (string)this["pattern"]; }
            set { this["pattern"] = value; }
        }

    }
}