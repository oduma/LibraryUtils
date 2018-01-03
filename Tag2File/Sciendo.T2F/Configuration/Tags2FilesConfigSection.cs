using System.Configuration;

namespace Sciendo.T2F.Configuration
{
    public class Tags2FilesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("rules")]
        public RuleElementCollection Rules => (RuleElementCollection)this["rules"];

        [ConfigurationProperty("action")]
        public ActionTypeConfigurationElement ActionType => (ActionTypeConfigurationElement)this["action"];

        [ConfigurationProperty("extensions")]
        public ExtensionElementCollection Extensions => (ExtensionElementCollection) this["extensions"];
    }


}

