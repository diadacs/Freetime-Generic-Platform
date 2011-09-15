using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Freetime.PluginManagement.Configuration
{
    public class PluginManagerConfigurationAttributeCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(PluginManagerConfigurationAttribute element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PluginManagerConfigurationAttribute();
        }

        public void Remove(PluginManagerConfigurationAttribute element)
        {
            BaseRemove(element.Key);
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
            return ((PluginManagerConfigurationAttribute)element).Key;
        }
    }
}
