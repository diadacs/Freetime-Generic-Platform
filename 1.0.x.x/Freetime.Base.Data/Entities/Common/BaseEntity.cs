using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Entities.Common
{
    [Serializable]
    [DataContract]
    public abstract class BaseEntity : IDisposable
    {

        [DataMember]
        [XmlElement("ID")]
        public virtual Int64 ID
        {
            get;
            protected set;
        }

        public virtual void Dispose()
        {
        }

    }
}
