using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Entities
{
    [Serializable]
    [DataContract]
    [XmlRoot("WebPartialView",
        Namespace = "http://www.freeG-businessplatform.com",
        IsNullable = true)]
    public class WebPartialView
    {
        
        public WebPartialView()
        {
            IsActive = true;
        }

        [DataMember]
        [XmlElement("Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlElement("File")]
        public string File { get; set; }

        [DataMember]
        [XmlElement("IsThemed")]
        public bool IsThemed { get; set; }

        [DataMember]
        [XmlElement("IsActive")]
        public bool IsActive { get; set; }

    }
}
