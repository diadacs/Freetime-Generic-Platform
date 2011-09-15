using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Anito.Data.Mapping;
using Anito.Data;

namespace Freetime.Base.Data.Entities.Common
{
    [Serializable]
    public class PublishableEntity : AuditableEntity, IPublishable
    {
        public PublishableEntity()
            : base()
        { }

        public void Publish()
        {
            IsPublished = true;
            DatePublished = DateTime.Now;
        }

        public void UnPublish()
        {
            IsPublished = false;
            DatePublished = null;
        }

        [XmlElementAttribute("IsPublished")]
        [DataField(FieldName = "IsPublished")]
        public bool IsPublished
        {
            get;
            private set;
        }

        [XmlElementAttribute("DatePublished")]
        [DataField(FieldName = "DatePublished")]
        public DateTime? DatePublished
        {
            get;
            private set;
        }
    }
}
