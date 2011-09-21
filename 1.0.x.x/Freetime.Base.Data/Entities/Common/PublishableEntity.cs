using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Entities.Common
{
    [Serializable]
    [DataContract]
    public class PublishableEntity : AuditableEntity, IPublishable
    {
        
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

        [XmlElement("IsPublished")]
        [DataMember]
        public bool IsPublished
        {
            get;
            private set;
        }

        [XmlElement("DatePublished")]
        [DataMember]
        public DateTime? DatePublished
        {
            get;
            private set;
        }
    }
}
