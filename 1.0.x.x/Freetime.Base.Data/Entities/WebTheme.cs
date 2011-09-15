using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Anito.Data.Mapping;
using Anito.Data;
using Freetime.Base.Data.Entities.Common;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Entities
{
    [DataContract]    
    public class WebTheme : AuditableEntity
    {
        public WebTheme()
            : base()
        { }
        
        [DataMember]
        [XmlElementAttribute("Theme")]
        [DataField(FieldName = "Theme")]
        public string Theme { get; set; }

        [DataMember]
        [XmlElementAttribute("DisplayValue")]
        [DataField(FieldName = "DisplayValue")]
        public string DisplayValue { get; set; }

        [DataMember]
        [XmlElementAttribute("IsActive")]
        [DataField(FieldName = "IsActive")]
        public bool IsActive { get; set; }

    }
}
