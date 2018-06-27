using System.Configuration;
using Sciendo.T2F.Processor;

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