using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Freetime.Configuration
{
    public class FreetimeConfiguration : System.Configuration.ConfigurationSection
    {
        [ConfigurationProperty("DefaultTheme", IsRequired = false)]
        public string DefaultTheme
        {
            get
            {
                return (string) this["DefaultTheme"];
            }
            set
            {
                this["DefaultTheme"] = value;
            }
        }

        [ConfigurationProperty("DefaultMasterPage", IsRequired = false)]
        public string DefaultMasterPage
        {
            get
            {
                return (string)this["DefaultMasterPage"];
            }
            set
            {
                this["DefaultMasterPage"] = value;
            }
        }

        [ConfigurationProperty("GlobalEventConfigurationSection", IsRequired = false)]
        public string GlobalEventConfigurationSection
        {
            get
            {
                if(this["GlobalEventConfigurationSection"] == null)
                    return string.Empty;
                return (string)this["GlobalEventConfigurationSection"];
            }
            set
            {
                this["GlobalEventConfigurationSection"] = value;
            }
        }

        [ConfigurationProperty("PluginManagementConfigurationSection", IsRequired = true)]
        public string PluginManagementConfigurationSection
        {
            get
            {
                return (string)this["PluginManagementConfigurationSection"];
            }
            set
            {
                this["PluginManagementConfigurationSection"] = value;
            }
        }

        [ConfigurationProperty("UsingDataSerivce", IsRequired = false)]
        public bool UsingDataService
        {
            get
            {
                this["UsingDataService"] = this["UsingDataService"] ?? false;
                return (bool) this["UsingDataService"];
            }
            set
            {
                this["UsingDataService"] = value;
            }
        }
    }
}
