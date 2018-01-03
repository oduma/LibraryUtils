using System.Configuration;

namespace Sciendo.T2F.Configuration
{
    public class ActionTypeConfigurationElement:ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = ActionType.None, IsKey = true, IsRequired = true)]
        public ActionType Type
        {
            get { return (ActionType)this["type"]; }
            set { this["type"] = value; }
        }

    }
}