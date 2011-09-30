using System;
using System.Xml.Serialization;
using Freetime.Base.Data.Entities.Common;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Entities
{
    [Serializable]
    [DataContract]
    [XmlRoot("EventHandler",
        Namespace = "http://www.freetime-generic.com",
        IsNullable = true)]
    public class EventHandler : AuditableEntity
    {
        [DataMember]
        [XmlElement("UniqueId")]
        public string UniqueId { get; set; }

        [DataMember]
        [XmlElement("Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlElement("EventHandlerClass")]
        public string EventHandlerClass { get; set; }

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

