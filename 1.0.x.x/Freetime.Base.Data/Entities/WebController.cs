using System;
using System.Xml.Serialization;
using Anito.Data.Mapping;
using Freetime.Base.Data.Entities.Common;

namespace Freetime.Base.Data.Entities
{           
    [Serializable]
    public class WebController : BaseEntity
    {
        public WebController()
        {
            IsActive = true;
        }

        [XmlElementAttribute("Name")]
        [DataField(FieldName = "Name")]
        public string Name { get; set; }

        [XmlElementAttribute("ControllerType")]
        [DataField(FieldName = "ControllerType")]
        public string ControllerType { get; set; }

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
