using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Freetime.Base.Data.Entities.Common;

namespace Freetime.Base.Data.Entities
{
    [DataContract]
    [Serializable]
    [XmlRoot("UserGroup",
        Namespace = "http://www.freeG-businessplatform.com",
        IsNullable = true)]
    public class UserGroup : AuditableEntity
    {      

        [DataMember]
        [XmlElement("Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlElement("Description")]
        public string Description { get; set; }

        [DataMember]
        [XmlElement("IsActive")]
        public bool IsActive { get; set; }

    }
}
