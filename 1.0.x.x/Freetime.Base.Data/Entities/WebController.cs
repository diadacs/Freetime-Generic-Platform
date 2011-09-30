using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Entities
{           
    [Serializable]
    [DataContract]
    [XmlRoot("WebController",
        Namespace = "http://www.freetime-generic.com",
        IsNullable = true)]
    public class WebController
    {
        public WebController()
        {
            IsActive = true;
        }

        [DataMember]
        [XmlElement("Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlElement("ControllerType")]
        public string ControllerType { get; set; }

        [DataMember]
        [XmlElement("Assembly")]
        public string Assembly { get; set; }

        [DataMember]
        [XmlElement("AssemblyLocation")]
        public string AssemblyLocation { get; set; }

        [DataMember]
        [XmlElement("IsActive")]
        public bool IsActive { get; set; }
    }
}
