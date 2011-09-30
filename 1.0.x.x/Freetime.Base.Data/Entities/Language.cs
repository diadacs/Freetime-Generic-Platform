using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Freetime.Base.Data.Entities.Common;

namespace Freetime.Base.Data.Entities
{
    [DataContract]
    [Serializable]
    [XmlRoot("Language",
        Namespace = "http://www.freetime-generic.com",
        IsNullable = true)]
    public class Language : AuditableEntity
    {
       
        [DataMember]
        [XmlElement("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }

        [DataMember]
        [XmlElement("IsActive")]
        public bool IsActive { get; set; }

    }
}
