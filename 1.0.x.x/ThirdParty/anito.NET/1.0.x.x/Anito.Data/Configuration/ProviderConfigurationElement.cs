/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 


using System.Configuration;

namespace Anito.Data.Configuration
{    
    public class ProviderConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["Name"];
            }
        }

        [ConfigurationProperty("Type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)this["Type"];
            }
        }

        [ConfigurationProperty("ConnectionString", IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return (string)this["ConnectionString"];
            }
        }

        [
        ConfigurationProperty("SchemaSource", IsDefaultCollection = false),
        ConfigurationCollection(typeof(SchemaSourceCollection), AddItemName = "addSource", ClearItemsName = "clearSource", RemoveItemName = "removeSource")
        ]
        public SchemaSourceCollection SchemaSourceCollection
        {
            get
            {
                return this["SchemaSource"] as SchemaSourceCollection;
            }
        }

    }
}
