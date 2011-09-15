using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Freetime.PluginManagement.Configuration
{   
    public class PluginManagerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("IsCustom")]
        public bool IsCustom
        {
            get
            {
                if (this["IsCustom"] == null)
                    return false;
                return (bool)this["IsCustom"];
            }
            set
            {
                this["IsCustom"] = value;
            }
        }

        [ConfigurationProperty("Type")]
        public string Type
        {
            get
            {
                if (this["Type"] == null)
                    return string.Empty;
                return (string)this["Type"];
            }
            set
            {
                this["Type"] = value;
            }
        }

        [
        ConfigurationProperty("Attributes", IsDefaultCollection = false),
        ConfigurationCollection(typeof(PluginManagerConfigurationAttributeCollection), AddItemName = "addAttribute", ClearItemsName = "clearAttributes", RemoveItemName = "removeAttribute")
        ]
        public PluginManagerConfigurationAttributeCollection Attributes
        {
            get
            {
                return this["Attributes"] as PluginManagerConfigurationAttributeCollection;
            }
        }
    }
}
