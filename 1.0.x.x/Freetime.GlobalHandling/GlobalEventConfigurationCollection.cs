using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Freetime.GlobalHandling
{
    public class GlobalEventConfigurationCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType  CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(EventHandlerElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EventHandlerElement();
        }

        public void Remove(EventHandlerElement element)
        {
            BaseRemove(element.Name);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EventHandlerElement)element).Name;
        }
    }
}
