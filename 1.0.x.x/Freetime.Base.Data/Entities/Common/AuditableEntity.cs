using System;
using System.Xml.Serialization;
using Anito.Data.Mapping;


namespace Freetime.Base.Data.Entities.Common
{
    [Serializable]
    public abstract class AuditableEntity : BaseEntity, IAuditable
    {

        protected AuditableEntity()
            : base()
        { }

        [XmlElementAttribute("UserCreated")]
        [DataField(FieldName = "UserCreated")]
        public virtual int UserCreated { get; set; }

        [XmlElementAttribute("UserModified")]
        [DataField(FieldName = "UserModified")]
        public virtual int UserModified { get; set; }

        [XmlElementAttribute("UserDeleted")]
        [DataField(FieldName = "UserDeleted")]
        public virtual int UserDeleted { get; set; }

        [XmlElementAttribute("DateCreated")]
        [DataField(FieldName = "DateCreated")]
        public virtual DateTime DateCreated { get; set; }

        [XmlElementAttribute("DateModified")]
        [DataField(FieldName = "DateModified")]
        public virtual DateTime DateModified { get; set; }

        [XmlElementAttribute("DateDeleted")]
        [DataField(FieldName = "DateDeleted")]
        public virtual DateTime DateDeleted { get; set; }

        [XmlElementAttribute("IsDeleted")]
        [DataField(FieldName = "IsDeleted")]
        public virtual bool IsDeleted { get; set; }
    }
}
