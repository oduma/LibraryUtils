using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.T2F.Configuration
{
    [ConfigurationCollection(typeof(ExtensionElement))]
    public class ExtensionElementCollection : ConfigurationElementCollection
    {
        public ExtensionElement this[int index]
        {
            get { return (ExtensionElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ExtensionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExtensionElement)element).Key;
        }
    }

}
