using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Freetime.Base.Data.Entities
{    
    [Serializable]
    public class DataSessionService
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Assembly")]
        public string Assembbly { get; set; }

        [XmlElement("Contract")]
        public string Contract { get; set; }

        [XmlElement("ContractAssembly")]
        public string ContractAssembly { get; set; }
    }
}
