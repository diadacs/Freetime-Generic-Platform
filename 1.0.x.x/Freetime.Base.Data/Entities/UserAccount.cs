using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Freetime.Base.Data.Entities.Common;

namespace Freetime.Base.Data.Entities
{
    [DataContract]
    [Serializable]
    [XmlRoot("UserAccount",
        Namespace = "http://www.freeG-businessplatform.com",
        IsNullable = true)]
    public class UserAccount : AuditableEntity
    {
        [DataMember]
        [XmlElement("LoginName")]
        public virtual string LoginName { get; set; }

        [DataMember]
        [XmlElement("Password")]
        public virtual string Password { get; set; }

        [DataMember]
        [XmlElement("Name")]
        public virtual string Name { get; set; }

        [DataMember]
        [XmlElement("UserProfile")]
        public virtual Int64 UserProfile { get; set; }

        [DataMember]
        [XmlElement("WebTheme")]
        public virtual int WebTheme { get; set; }

        [DataMember]
        [XmlElement("Theme")]
        public virtual int Theme { get; set; }

        [DataMember]
        [XmlElement("IsActive")]
        public virtual bool IsActive { get; set; }
    }
}
