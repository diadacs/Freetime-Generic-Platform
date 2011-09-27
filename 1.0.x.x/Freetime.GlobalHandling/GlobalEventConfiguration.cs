using System.Configuration;

namespace Freetime.GlobalHandling
{
    public class GlobalEventConfiguration : ConfigurationSection
    {

        [
        ConfigurationProperty("Handlers", IsDefaultCollection=false),
        ConfigurationCollection(typeof(GlobalEventConfigurationCollection), AddItemName="addHandler", ClearItemsName="clearHandlers", RemoveItemName="removeHandler")
        ]
        public GlobalEventConfigurationCollection EventHandlerCollection
        {
            get
            {
                return this["Handlers"] as GlobalEventConfigurationCollection;
            }
        }
    }
}
