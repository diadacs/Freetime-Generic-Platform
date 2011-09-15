using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Freetime.GlobalHandling
{
    public class EventHandlerElement : ConfigurationElement
    {
        [ConfigurationProperty("handler", IsRequired=true)]
        public string Handler
        {
            get
            {
                return (string)this["handler"];
            }
            set
            {
                this["handler"] = value;
            }
        }

        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get
            {
                return (string)this["assembly"];
            }
            set
            {
                this["assembly"] = value;
            }
        }

        [ConfigurationProperty("assemblylocation", IsRequired = true)]
        public string AssemblyLocation
        {
            get
            {
                return (string)this["assemblylocation"];
            }
            set
            { 
                this["assemblylocation"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired=true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("isactive", IsRequired=false)]
        public bool IsActive
        {
            get
            {
                return (bool)this["isactive"];
            }
            set
            {
                this["isactive"] = value;
            }
        }
    }
}
