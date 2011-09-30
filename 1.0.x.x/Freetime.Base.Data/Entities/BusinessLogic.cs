using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Entities
{
    [Serializable]
    [DataContract]
    [XmlRoot("BusinessLogic",
        Namespace = "http://www.freetime-generic.com",
        IsNullable = true)]
    public class BusinessLogic 
    {
        public BusinessLogic()
        {
            IsActive = true;
        }

        [DataMember]
        [XmlElement("Type")]
        public string Type { get; set; }

        [DataMember]
        [XmlElement("Assembly")]
        public string Assembly { get; set; }

        [DataMember]
        [XmlElement("AssemblyLocation")]
        public string AssemblyLocation { get; set; }

        [DataMember]
        [XmlElement("DataSessionType")]
        public string DataSessionType { get; set; }

        [DataMember]
        [XmlElement("DataSessionAssembly")]
        public string DataSessionAssembly { get; set; }

        [DataMember]
        [XmlElement("DataSessionAssemblyLocation")]
        public string DataSessionAssemblyLocation { get; set; }

        [DataMember]
        [XmlElement("IsActive")]
        public bool IsActive { get; set; }
    }
}
