using System.Configuration;

namespace Sciendo.T2F.Configuration
{
    [ConfigurationCollection(typeof(RuleElement))]
    public class RuleElementCollection:ConfigurationElementCollection
    {
        public RuleElement this[int index]
        {
            get { return (RuleElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleElement)element).Type;
        }

    }
}