using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Anito.Data.Mapping;
using Freetime.Base.Data.Entities.Common;

namespace Freetime.Base.Data.Entities
{
    [Serializable]
    public class EventHandler : AuditableEntity
    {
        public EventHandler()
            : base()
        { }

        [XmlElementAttribute("UniqueId")]
        [DataField(FieldName = "UniqeID"
            , PrimaryKey = true)]
        public string UniqueID { get; set; }

        [XmlElementAttribute("Name")]
        [DataField(FieldName = "Name")]
        public string Name { get; set; }

        [XmlElementAttribute("EventHandlerClass")]
        [DataField(FieldName = "EventHandlerClass")]
        public string EventHandlerClass { get; set; }

        [XmlElementAttribute("Assembly")]
        [DataField(FieldName = "Assembly")]
        public string Assembly { get; set; }

        [XmlElementAttribute("AssemblyLocation")]
        [DataField(FieldName = "AssemblyLocation")]
        public string AssemblyLocation { get; set; }

        [XmlElementAttribute("IsActive")]
        [DataField(FieldName = "IsActive")]
        public bool IsActive { get; set; }

    }
}

