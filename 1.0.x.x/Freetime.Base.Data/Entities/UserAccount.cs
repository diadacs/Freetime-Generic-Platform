using Anito.Data.Mapping;
using Freetime.Base.Data.Entities.Common;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Entities
{

    [DataContract]
    [Source(View = "UserAccountView",
        Update = "UserAccount")]
    public class UserAccount : AuditableEntity
    {

        [DataMember]
        [DataField(FieldName = "ID"
            , MemberName = "_ID"
            , Identity = true
            , PrimaryKey = true
            )]
        public virtual int ID { get; private set; }

        [DataMember]
        [DataField(FieldName = "LoginName", Size = 250
            )]
        public virtual string LoginName { get; set; }

        [DataMember]
        [DataField(FieldName = "Password", Size = 250
            )]
        public virtual string Password { get; set; }

        [DataMember]
        [DataField(FieldName = "UserRole")]
        public virtual int UserRole { get; set; }

        [DataMember]
        [DataField(FieldName = "Name", Size = 250
            )]
        public virtual string Name { get; set; }

        [DataMember]
        [DataField(FieldName = "UserProfile")]
        public virtual int UserProfile { get; set; }

        [DataMember]
        [DataField(FieldName = "WebTheme")]
        public virtual int WebTheme { get; set; }

        [DataMember]
        [DataField(FieldName = "Theme")]
        public virtual string Theme { get; set; }

        [DataMember]
        [DataField(FieldName = "IsActive")]
        public virtual bool IsActive { get; set; }
    }
}
