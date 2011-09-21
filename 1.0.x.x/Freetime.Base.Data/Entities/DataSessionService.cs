using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Entities
{    
    [Serializable]
    [DataContract]
    [XmlRoot("DataSessionService",
        Namespace = "http://www.freeG-businessplatform.com",
        IsNullable = true)]
    public class DataSessionService
    {
        [XmlElement("Name")]
        [DataMember]
        public string Name { get; set; }

        [XmlElement("Type")]
        [DataMember]
        public string Type { get; set; }

        [XmlElement("Assembly")]
        [DataMember]
        public string Assembbly { get; set; }

        [XmlElement("Contract")]
        [DataMember]
        public string Contract { get; set; }

        [XmlElement("ContractAssembly")]
        [DataMember]
        public string ContractAssembly { get; set; }
    }
}
