using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Anito.Data.Mapping;
using Anito.Data;
using Freetime.Base.Data.Entities.Common;

namespace Freetime.Base.Data.Entities
{
    [Serializable]
    public class WebMasterPage : AuditableEntity
    {
        
        public WebMasterPage()
            : base()
        {
            IsActive = true;
        }

        [XmlElementAttribute("Name")]
        [DataField(FieldName = "Name")]
        public string Name { get; set; }

        [XmlElementAttribute("File")]
        [DataField(FieldName = "File")]
        public string File { get; set; }

        [XmlElementAttribute("IsThemed")]
        [DataField(FieldName = "IsThemed")]
        public bool IsThemed { get; set; }

        [XmlElementAttribute("IsActive")]
        [DataField(FieldName = "IsActive")]
        public bool IsActive { get; set; }
    }
}
