using System.Configuration;

namespace Freetime.PluginManagement.Configuration
{
    public class PluginManagerConfigurationAttribute : ConfigurationElement
    {
        [ConfigurationProperty("Key", IsRequired = true)]
        public string Key
        {
            get
            {
                return (string)this["Key"];
            }
            set
            {
                this["Key"] = value;
            }
        }

        [ConfigurationProperty("Value", IsRequired = true)]
        public string Value
        {
            get
            {
                return (string) this["Value"];
            }
            set
            {
                this["Value"] = value;
            }
        }
    }
}
