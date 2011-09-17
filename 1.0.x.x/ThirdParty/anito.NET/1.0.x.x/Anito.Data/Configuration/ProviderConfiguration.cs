/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Configuration;

namespace Anito.Data.Configuration
{
    public class ProviderConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("DefaultProvider")]
        public string DefaultProvider
        {
            get
            {
                if (this["DefaultProvider"] == null)
                    throw new Exception("Default Provider not defined.");
                return (string)this["DefaultProvider"];
            }
        }

        [
        ConfigurationProperty("Providers", IsDefaultCollection = false),
        ConfigurationCollection(typeof(ProviderConfigurationElementCollection), AddItemName = "Provider", ClearItemsName = "ClearProviders", RemoveItemName = "RemoveProvider")
        ]
        public ProviderConfigurationElementCollection Providers
        {
            get
            {
                return this["Providers"] as ProviderConfigurationElementCollection;
            }
        }
    }
}
