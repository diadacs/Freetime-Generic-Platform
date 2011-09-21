using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Entities
{
    [DataContract]    
    [Serializable]
    [XmlRoot("WebTheme",
        Namespace = "http://www.freeG-businessplatform.com",
        IsNullable = true)]
    public class WebTheme 
    {
        
        [DataMember]
        [XmlElement("Theme")]
        public string Theme { get; set; }

        [DataMember]
        [XmlElement("DisplayValue")]
        public string DisplayValue { get; set; }

        [DataMember]
        [XmlElement("IsActive")]
        public bool IsActive { get; set; }

    }
}
